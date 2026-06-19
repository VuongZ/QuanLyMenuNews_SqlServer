using Application.DTO;
using MediatR;

namespace Application.Requests.XuLyMenuNews
{
    public class GetAllMenuNewsRequest : IRequest<IEnumerable<MenuNewsResponseDto>>
    {
    }
}
