using MediatR;

namespace Application.Requests.XuLyMenuNews
{
    public class DeleteMenuNewsRequest : IRequest<bool>
    {
        public int MenuId { get; set; }
        public int NewsId { get; set; }
    }
}
