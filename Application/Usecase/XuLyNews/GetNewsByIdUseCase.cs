using Application.DTO;
using Application.Requests.XuLyNews;
using Domain.entity;
using Domain.repositories;
using MediatR;

namespace Application.Usecase.XuLyNews
{
    public class GetNewsByIdUseCase : IRequestHandler<GetNewsByIdRequest, NewsResponseDto?>
    {
        private readonly INewsRepo newsRepo;
        public GetNewsByIdUseCase(INewsRepo Repo)
        {
            newsRepo = Repo;
        }
        public async Task<NewsResponseDto?> Handle(GetNewsByIdRequest request,CancellationToken cancellationToken)
        {
            var n = await newsRepo.GetByIdWithMenusAsync(request.id);
            if(n==null)
                return null;
            return new NewsResponseDto{
                    Id        = n.Id,
                    Title     = n.Title,
                    Slug      = n.Slug,
                    Content   = n.Content,
                    Thumbnail = n.Thumbnail,
                    Address   = n.Address,
                    WardId    = n.WardId,
                    FullAddress = n.Ward == null? n.Address: n.Address + ", " + n.Ward.FullName,
                    WardInfo = n.Ward == null ? null : new WardInfoResponseDto
                    {
                        WardId     = n.Ward.WardId,
                        WardPid    = n.Ward.WardPid,
                        Name       = n.Ward.Name,
                        NameEn     = n.Ward.NameEn,
                        FullName   = n.Ward.FullName,
                        FullNameEn = n.Ward.FullNameEn,
                        Country    = n.Ward.Localization.Localization ,
                        WardParent = new WardParentResponseDto{
                            WardId = n.Ward.WardPid,
                            Name   = n.Ward.FullName != null && n.Ward.FullName.Contains(",")? n.Ward.FullName.Substring(n.Ward.FullName.IndexOf(",") + 1).Trim(): string.Empty,
                            NameEn = n.Ward.FullNameEn != null && n.Ward.FullNameEn.Contains(",")? n.Ward.FullNameEn.Substring(n.Ward.FullNameEn.IndexOf(",") + 1).Trim(): null,
                            Country = n.Ward.Localization?.Localization,
                        }
                    },  
                    Menus = n.Menu.Select(m => new MenuBasicResponseDto
                    {
                        Id   = m.Id,
                        Name = m.Name,
                        Slug = m.Slug,
                    })
            };
        }
}
}