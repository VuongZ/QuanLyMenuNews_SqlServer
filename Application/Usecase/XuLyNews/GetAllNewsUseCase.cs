using Application.DTO;
using Application.XuLyNews.Requests;
using Domain.entity;
using Domain.repositories;
using MediatR;

namespace Application.XuLyNews.UsesCases
{
    
    public class GetAllNewsUseCase : IRequestHandler<GetAllNewsRequest, IEnumerable<NewsResponseDto>>
    {
        private readonly INewsRepo _newsRepo;

        public GetAllNewsUseCase(INewsRepo newsRepo)
        {
            _newsRepo = newsRepo;
        }

        public async Task<IEnumerable<NewsResponseDto>> Handle(GetAllNewsRequest request, CancellationToken cancellationToken)
        {
 
        var news = await _newsRepo.GetAllWithMenusAsync();
            return news.Select(n => new NewsResponseDto
            {
                Id = n.Id,
                Title = n.Title ?? string.Empty,
                Slug = n.Slug ?? string.Empty,
                Content = n.Content ?? string.Empty,
                Thumbnail = n.thumbnail ?? string.Empty,
                   Address = n.Address,
                WardId = n.WardId,
                 FullAddress = n.Ward == null
                    ? n.Address
                    : string.Join(
                        ", ",
                        new[]
                        {
                            n.Address,
                            n.Ward.FullName,
                            "Việt Nam"
                        }
                        .Where(x =>
                            !string.IsNullOrWhiteSpace(x))
                    ),

                WardInfo = n.Ward == null
                    ? null
                    : new WardInfoResponseDto
                    {
                        WardId = n.Ward.WardId,
                        WardPid = n.Ward.WardPid,
                        Name = n.Ward.Name,
                        NameEn = n.Ward.NameEn,
                        FullName = n.Ward.FullName,
                        FullNameEn = n.Ward.FullNameEn,
                        KeyLocalization =
                            n.Ward.KeyLocalization,

                        WardParent =
                            new WardParentResponseDto
                            {
                                WardId = n.Ward.WardPid,

                                Name = n.Ward.FullName
                                    .Contains(",")
                                    ? n.Ward.FullName
                                        .Split(',', 2)[1]
                                        .Trim()
                                    : string.Empty,

                                NameEn =
                                    !string.IsNullOrWhiteSpace(
                                        n.Ward.FullNameEn)
                                    && n.Ward.FullNameEn
                                        .Contains(",")
                                        ? n.Ward.FullNameEn
                                            .Split(',', 2)[1]
                                            .Trim()
                                        : null,

                                FullName = n.Ward.FullName
                                    .Contains(",")
                                    ? n.Ward.FullName
                                        .Split(',', 2)[1]
                                        .Trim()
                                    : string.Empty
                            }
                    },

                Menus = n.Menu.Select(m => new MenuBasicResponseDto
                {
                    Id = m.Id,
                    Name = m.Name ?? string.Empty,
                    Slug = m.Slug ?? string.Empty,
                }).ToList()
            });
        }
    }
}