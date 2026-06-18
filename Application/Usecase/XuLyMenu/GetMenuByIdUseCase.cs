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
                        Thumbnail = n.thumbnail ?? string.Empty
                    }).ToList()
                }
                : null;
        }
    }
};