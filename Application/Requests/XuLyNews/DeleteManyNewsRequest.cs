using MediatR;

namespace Application.Requests.XuLyNews;

public class DeleteManyNewsRequest : IRequest<int>
{
    public List<int> Ids { get; set; } = new();
}