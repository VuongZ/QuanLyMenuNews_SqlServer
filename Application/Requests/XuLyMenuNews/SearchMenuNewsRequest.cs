using Application.DTO;
using MediatR;

namespace Application.Requests.XuLyMenuNews
{
    public class SearchMenuNewsRequest : IRequest<IEnumerable<MenuNewsResponseDto>>
    {
        public int? MenuId { get; set; }
        public int? NewsId { get; set; }
    }
}
