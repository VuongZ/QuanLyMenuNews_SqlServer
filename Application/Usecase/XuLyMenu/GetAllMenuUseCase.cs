using MediatR;
using Domain.entity;
using Domain.repositories;
using Application.Requests.XuLyMenu;
using Application.DTO;
using System.Runtime.CompilerServices;

namespace Application.XuLyMenu.UseCases
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
                    News = m.News.Select(n => new NewsResponseDto
                {
                    Id        = n.Id,
                    Title     = n.Title ?? string.Empty,
                    Slug      = n.Slug ?? string.Empty,
                    Content   = n.Content,
                    Thumbnail = n.Thumbnail,
                    Address   = n.Address,
                    WardId    = n.WardId,
                FullAddress = n.Ward == null ? n.Address: n.Address + ", " + n.Ward.FullName,
            WardInfo = n.Ward == null ? null : new WardInfoResponseDto
            {
                WardId     = n.Ward.WardId,
                WardPid    = n.Ward.WardPid,
                Name       = n.Ward.Name ?? string.Empty,
                NameEn     = n.Ward.NameEn,
                FullName   = n.Ward.FullName ?? string.Empty,
                FullNameEn = n.Ward.FullNameEn,
                Country    = n.Ward.Localization?.Localization ?? string.Empty,
                WardParent = new WardParentResponseDto
            {
                WardId = n.Ward.WardPid,
                Name = n.Ward.FullName != null && n.Ward.FullName.Contains(",")
                    ? n.Ward.FullName.Substring(n.Ward.FullName.IndexOf(",") + 1).Trim()
                    : string.Empty,
                NameEn = n.Ward.FullNameEn != null && n.Ward.FullNameEn.Contains(",")
                    ? n.Ward.FullNameEn.Substring(n.Ward.FullNameEn.IndexOf(",") + 1).Trim()
                    : null,
                Country = n.Ward.Localization?.Localization ?? string.Empty,
            }}
                })
                });

                
        }

    }
}