using Application.DTO;
using Domain.entity;
using MediatR;

namespace Application.Requests.XuLyMenu
{
    public class GetAllMenuRequest : IRequest<IEnumerable< MenuResponseDto>>
    {
          
    }
}