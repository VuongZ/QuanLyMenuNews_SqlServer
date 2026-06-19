using Application.DTO;
using Application.Requests.XuLyMenu;
using Domain.entity;
using Domain.repositories;
using MediatR;

namespace Application.Usecase.XuLyMenu
{
    public class GetMenuByIdUseCase : IRequestHandler<GetMenuByIdRequest, MenuResponseDto?>
    {
        private readonly IMenuRepo _menuRepo;
        
        public GetMenuByIdUseCase(IMenuRepo menuRepo)
        {
            _menuRepo = menuRepo;
        }

        public async Task<MenuResponseDto?> Handle(GetMenuByIdRequest request, CancellationToken cancellationToken)
        {
            var menu = await _menuRepo.GetByIdWithNewsAsync(request.id);
            return menu != null ? new MenuResponseDto
            {
                Id = menu.Id,
                Name = menu.Name ?? string.Empty,
                Slug = menu.Slug ?? string.Empty,
                News = menu.News.Select(n => new NewsResponseDto
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
                }
                : null;
        }
    }
};