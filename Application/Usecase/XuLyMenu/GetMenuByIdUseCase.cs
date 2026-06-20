using Application.DTO;
using Application.Interfaces;
using Application.Requests.XuLyMenu;
using Domain.entity;
using Domain.repositories;
using MediatR;

namespace Application.Usecase.XuLyMenu
{
    public class GetMenuByIdUseCase : IRequestHandler<GetMenuByIdRequest, MenuResponseDto?>
    {
        private readonly IMenuQueryRepository  _menuRepo;
        
        public GetMenuByIdUseCase(IMenuQueryRepository  menuRepo)
        {
            _menuRepo = menuRepo;
        }

        public async Task<MenuResponseDto?> Handle(GetMenuByIdRequest request, CancellationToken cancellationToken)
        {
            return await _menuRepo.GetByIdWithNewsAsync(request.id);
        }
    }
}