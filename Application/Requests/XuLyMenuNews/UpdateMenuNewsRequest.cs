using MediatR;

namespace Application.Requests.XuLyMenuNews
{
    public class UpdateMenuNewsRequest : IRequest<bool>
    {
        public int OldMenuId { get; set; }
        public int OldNewsId { get; set; }
        public int MenuId { get; set; }
        public int NewsId { get; set; }
    }
}
