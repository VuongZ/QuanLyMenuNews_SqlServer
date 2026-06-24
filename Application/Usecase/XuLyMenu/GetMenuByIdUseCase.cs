using Application.DTO;
using Application.Mappers;
using Application.Requests.XuLyMenu;
using Domain.entity;
using Domain.repositories;
using MediatR;

namespace Application.Usecase.XuLyMenu
{
    public class GetMenuByIdUseCase : IRequestHandler<GetMenuByIdRequest, MenuResponseDto?>
    {
        private readonly IMenuRepo  _menuRepo;
        
        public GetMenuByIdUseCase(IMenuRepo  menuRepo)
        {
            _menuRepo = menuRepo;
        }
        
        public async Task<MenuResponseDto?> Handle(GetMenuByIdRequest request, CancellationToken cancellationToken)
         {
            var m = await _menuRepo.GetByIdWithNewsAsync(request.id);
            if (m == null) return null;
            return m.ToDto();
         }
    }
}