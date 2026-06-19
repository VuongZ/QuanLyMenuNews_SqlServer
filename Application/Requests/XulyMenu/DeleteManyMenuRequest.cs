using MediatR;

namespace Application.Requests.XuLyMenu;

public class DeleteManyMenuRequest : IRequest<int>
{
    public List<int> Ids { get; set; } = new();
}