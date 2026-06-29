using MediatR;

namespace Application.XuLyNews.Requests;
public class RestoreNewsRequest : IRequest<bool>
{
    public int Id { get; set; }
}