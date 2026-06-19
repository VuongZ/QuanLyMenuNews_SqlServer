using MediatR;
using Domain.entity;
using Domain.repositories;
using Application.Requests.XuLyMenu;
using Application.DTO;

namespace Application.XuLyMenu.UseCases
{
 
    public class GetAllMenuUseCase : IRequestHandler<GetAllMenuRequest, IEnumerable<MenuResponseDto ?>>
    {
        private readonly IMenuRepo _menuRepo;


        public GetAllMenuUseCase(IMenuRepo menuRepo)
        {
            _menuRepo = menuRepo;

        }


        public async Task<IEnumerable<MenuResponseDto?>> Handle(GetAllMenuRequest request, CancellationToken cancellationToken)
        {
           
            var menus = await _menuRepo.GetAllWithNewsAsync();
            return menus.Select(m => new MenuResponseDto
            {
                Id = m.Id,
                Name = m.Name ?? string.Empty,
                Slug = m.Slug ?? string.Empty,
                News = m.News.Select(n => new NewsResponseDto
                {
                    Id = n.Id,
                    Title = n.Title ?? string.Empty,
                    Slug = n.Slug ?? string.Empty,
                    Content = n.Content ?? string.Empty,
                    Thumbnail = n.thumbnail ?? string.Empty,
                     Address = n.Address,
                    WardId = n.WardId,FullAddress = n.Ward == null
                        ? n.Address
                        : string.Join(", ",
                            new[]
                            {
                                n.Address,
                                n.Ward.FullName,
                                "Việt Nam"
                            }
                            .Where(x => !string.IsNullOrWhiteSpace(x))
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
                            WardParent = new WardParentResponseDto
                            {
                                WardId = n.Ward.WardPid,

                                Name = n.Ward.FullName.Contains(",")
                                    ? n.Ward.FullName.Split(',', 2)[1].Trim()
                                    : string.Empty,

                                NameEn = !string.IsNullOrWhiteSpace(n.Ward.FullNameEn)
                                    && n.Ward.FullNameEn.Contains(",")
                                        ? n.Ward.FullNameEn.Split(',', 2)[1].Trim()
                                        : null,

                                FullName = n.Ward.FullName.Contains(",")
                                    ? n.Ward.FullName.Split(',', 2)[1].Trim()
                                    : string.Empty
                            }
                        }

                }).ToList()
            });
        }
    }
}