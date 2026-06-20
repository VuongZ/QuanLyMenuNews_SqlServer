using Application.DTO;
using Domain.entity;
using MediatR;

namespace Application.Requests.XuLyMenu
{
    public class GetAllMenuRequest : IRequest<IAsyncEnumerable< MenuResponseDto>>
    {
          
    }
}