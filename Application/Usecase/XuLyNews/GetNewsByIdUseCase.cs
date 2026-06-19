using Application.DTO;
using Application.Requests.XuLyNews;
using Domain.entity;
using Domain.repositories;
using MediatR;

namespace Application.Usecase.XuLyNews
{
    public class GetNewsByIdUseCase : IRequestHandler<GetNewsByIdRequest, NewsResponseDto?>
    {
        private readonly INewsRepo _newsRepo;
        public GetNewsByIdUseCase(INewsRepo newsRepo)
        {
            _newsRepo = newsRepo;
        }
        public async Task<NewsResponseDto?> Handle(GetNewsByIdRequest request, CancellationToken cancellationToken)
        {

            var news = await _newsRepo.GetByIdWithMenusAsync(request.id);
            if (news != null)            {
                return new NewsResponseDto
            {
                Id = news.Id,
                Title = news.Title ?? string.Empty,
                Slug = news.Slug ?? string.Empty,
                Content = news.Content ?? string.Empty,
                Thumbnail = news.thumbnail ?? string.Empty,
                 Address = news.Address,
                WardId = news.WardId,

                FullAddress = news.Ward == null
                    ? news.Address
                    : string.Join(
                        ", ",
                        new[]
                        {
                            news.Address,
                            news.Ward.FullName,
                            "Việt Nam"
                        }
                        .Where(x =>
                            !string.IsNullOrWhiteSpace(x))
                    ),

                WardInfo = news.Ward == null
                    ? null
                    : new WardInfoResponseDto
                    {
                        WardId = news.Ward.WardId,
                        WardPid = news.Ward.WardPid,
                        Name = news.Ward.Name,
                        NameEn = news.Ward.NameEn,
                        FullName = news.Ward.FullName,
                        FullNameEn = news.Ward.FullNameEn,
                        KeyLocalization =
                            news.Ward.KeyLocalization,

                        WardParent =
                            new WardParentResponseDto
                            {
                                WardId = news.Ward.WardPid,

                                Name = news.Ward.FullName
                                    .Contains(",")
                                    ? news.Ward.FullName
                                        .Split(',', 2)[1]
                                        .Trim()
                                    : string.Empty,

                                NameEn =
                                    !string.IsNullOrWhiteSpace(
                                        news.Ward.FullNameEn)
                                    && news.Ward.FullNameEn
                                        .Contains(",")
                                        ? news.Ward.FullNameEn
                                            .Split(',', 2)[1]
                                            .Trim()
                                        : null,

                                FullName = news.Ward.FullName
                                    .Contains(",")
                                    ? news.Ward.FullName
                                        .Split(',', 2)[1]
                                        .Trim()
                                    : string.Empty
                            }
                    },
                Menus = news.Menu.Select(m => new MenuBasicResponseDto
                {
                    Id = m.Id,
                    Name = m.Name ?? string.Empty,
                    Slug = m.Slug ?? string.Empty,
                }).ToList()
            };
            }
            return null;
          
    }
}
}