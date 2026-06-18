using Application.DTO;
using Domain.entity;
using MediatR;

namespace Application.Requests.XuLyMenu
{
    public class GetMenuByIdRequest : IRequest<MenuResponseDto>
    {
        public int id { get; set; }
    }
}