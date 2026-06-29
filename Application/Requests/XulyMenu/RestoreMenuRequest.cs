using MediatR;

namespace Application.XuLyMenu.Requests;
public class RestoreMenuRequest : IRequest<bool>
{
    public int Id { get; set; }
}