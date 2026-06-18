using MediatR;

namespace Application.Requests.XuLyMenu
{
    public class DeleteMenuRequest : IRequest<bool>

    {
        public int Id { get; set; }
    }
}