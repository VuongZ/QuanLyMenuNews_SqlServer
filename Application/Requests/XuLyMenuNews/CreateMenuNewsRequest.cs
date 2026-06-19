using MediatR;

namespace Application.Requests.XuLyMenuNews
{
    public class CreateMenuNewsRequest : IRequest<bool>
    {
        public int MenuId { get; set; }
        public int NewsId { get; set; }
    }
}
