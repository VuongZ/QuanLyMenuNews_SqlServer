using MediatR;
using Domain.entity;
using Domain.repositories;
using Application.Requests.XuLyMenu;
using Application.DTO;
using Application.Interfaces;

namespace Application.XuLyMenu.UseCases
{
 
    public class GetAllMenuUseCase : IRequestHandler<GetAllMenuRequest, IAsyncEnumerable<MenuResponseDto>>
    {
        private readonly IMenuQueryRepository _menuRepo;


        public GetAllMenuUseCase(IMenuQueryRepository menuRepo)
        {
            _menuRepo = menuRepo;

        }


        public Task<IAsyncEnumerable<MenuResponseDto>> Handle(GetAllMenuRequest request, CancellationToken cancellationToken)
        {
           var menus=_menuRepo.GetAllWithNewsAsync();
           return Task.FromResult(menus);
        }
    }
}