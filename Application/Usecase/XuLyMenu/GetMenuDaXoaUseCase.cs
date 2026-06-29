using MediatR;
using Domain.repositories;
using Application.Requests.XuLyMenu;
using Application.DTO;

namespace Application.News.XuLyMenu.UseCases
{

    public class GetMenuDaXoa : IRequestHandler<GetMenuDaXoaRequest, IEnumerable<MenuBasicResponseDto>>
    {
        private readonly IMenuRepo menuRepo;
        public GetMenuDaXoa(IMenuRepo Repo)
        {
            menuRepo = Repo;
        }


    public async Task<IEnumerable<MenuBasicResponseDto>> Handle(GetMenuDaXoaRequest request,CancellationToken cancellationToken)
        {
            var menus = await menuRepo.GetDaXoa();
            return menus.Select(m => new MenuBasicResponseDto
                {
                    Id   = m.Id,
                    Name = m.Name,
                    Slug = m.Slug,
                }).AsEnumerable();
            
        }
    }
}