using System.Runtime.CompilerServices;
using Application.DTO;
using Application.XuLyNews.Requests;
using Domain.entity;
using Domain.repositories;
using MediatR;

namespace Application.XuLyNews.UsesCases
{
    
    public class GetAllNewsUseCase : IRequestHandler<GetAllNewsRequest, IEnumerable<NewsResponseDto>>
    {
        private readonly INewsRepo newsRepo;

        public GetAllNewsUseCase(INewsRepo Repo)
        {
            newsRepo = Repo;
        }

        public async Task<IEnumerable<NewsResponseDto>> Handle(GetAllNewsRequest request, CancellationToken cancellationToken)
        {
            return newsRepo.GetAllWithMenusAsync()
            .Select(n => new NewsResponseDto{
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
                            Country = n.Ward.Localization.Localization,
                        }
                    },  
                    Menus = n.MenuNews
                    .Where(m=>!m.Menu.IsDeleted)
                    .Select(m => new MenuBasicResponseDto
                    {
                        Id   = m.Menu.Id,
                        Name = m.Menu.Name,
                        Slug = m.Menu.Slug,
                    })
            }
            ).AsEnumerable();
        }

    }
}
