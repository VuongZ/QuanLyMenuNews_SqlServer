using MediatR;
using Domain.entity;
using Domain.repositories;
using Application.Requests.XuLyMenu;
using Application.DTO;
using System.Runtime.CompilerServices;
using Application.Mappers;

namespace Application.XuLyMenu.UseCases
{
 
    public class GetAllMenuUseCase : IRequestHandler<GetAllMenuRequest, IAsyncEnumerable<MenuResponseDto>>
    {
        private readonly IMenuRepo _menuRepo;


        public GetAllMenuUseCase(IMenuRepo menuRepo)
        {
            _menuRepo = menuRepo;

        }

        public Task<IAsyncEnumerable<MenuResponseDto>> Handle( GetAllMenuRequest request,CancellationToken cancellationToken)
            {
                return Task.FromResult(StreamMenus(cancellationToken));
            }

        private async IAsyncEnumerable<MenuResponseDto> StreamMenus([EnumeratorCancellation] CancellationToken cancellationToken)
{
              {
            await foreach (var m in _menuRepo.GetAllWithNewsAsync().WithCancellation(cancellationToken))
            {
                yield return m.ToDto();
            }
        }
        }
    }
}