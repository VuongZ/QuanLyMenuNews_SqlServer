using MediatR;
using Domain.entity;
using Domain.repositories;
using Application.Requests.XuLyMenu;
using Application.DTO;
using System.Runtime.CompilerServices;

namespace Application.News.XuLyMenu.UseCases
{

    public class GetAllMenuUseCase : IRequestHandler<GetAllMenuRequest, IEnumerable<MenuResponseDto>>
    {
        private readonly IMenuRepo menuRepo;
        public GetAllMenuUseCase(IMenuRepo Repo)
        {
            menuRepo = Repo;
        }

        public async Task<IEnumerable<MenuResponseDto>> Handle( GetAllMenuRequest request,CancellationToken cancellationToken)
        {
              return  menuRepo.GetAllWithNewsAsync()
                .Select(m => new MenuResponseDto
                {
                    Id   = m.Id,
                    Name = m.Name,
                    Slug = m.Slug,
                    News = m.MenuNews.Select(n => new NewsResponseDto
                {
                    Id        = n.News.Id,
                    Title     = n.News.Title ?? string.Empty,
                    Slug      = n.News.Slug ?? string.Empty,
                    Content   = n.News.Content,
                    Thumbnail = n.News.Thumbnail,
                    Address   = n.News.Address,
                    WardId    = n.News.WardId,
                FullAddress = n.News.Ward == null ? n.News.Address: n.News.Address + ", " + n.News.Ward.FullName,
            WardInfo = n.News.Ward == null ? null : new WardInfoResponseDto
            {
                WardId     = n.News.Ward.WardId,
                WardPid    = n.News.Ward.WardPid,
                Name       = n.News.Ward.Name ?? string.Empty,
                NameEn     = n.News.Ward.NameEn,
                FullName   = n.News.Ward.FullName ?? string.Empty,
                FullNameEn = n.News.Ward.FullNameEn,
                Country    = n.News.Ward.Localization.Localization,
                WardParent = new WardParentResponseDto
            {
                WardId = n.News.Ward.WardPid,
                Name = n.News.Ward.FullName != null && n.News.Ward.FullName.Contains(",")? n.News.Ward.FullName.Substring(n.News.Ward.FullName.IndexOf(",") + 1).Trim(): string.Empty,
                NameEn = n.News.Ward.FullNameEn != null && n.News.Ward.FullNameEn.Contains(",")? n.News.Ward.FullNameEn.Substring(n.News.Ward.FullNameEn.IndexOf(",") + 1).Trim(): null,
                Country = n.News.Ward.Localization.Localization,
            }}
                })
                }).AsEnumerable();

                
        }

    }
}