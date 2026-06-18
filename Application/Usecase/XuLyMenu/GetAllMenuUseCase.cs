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
            var page = request.Page <= 0 ? 1 : request.Page;
var pageSize = request.PageSize <= 0 ? 10 : request.PageSize;
pageSize = pageSize > 50 ? 50 : pageSize;
            var menus = await _menuRepo.GetAllWithNewsAsync(page, pageSize);
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
                    Thumbnail = n.thumbnail ?? string.Empty
                }).ToList()
            });
        }
    }
}