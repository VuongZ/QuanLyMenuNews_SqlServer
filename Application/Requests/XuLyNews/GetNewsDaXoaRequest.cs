using Application.DTO;
using MediatR;

namespace Application.XuLyNews.Requests
{
    public class GetNewsDaXoaRequest : IRequest<IEnumerable<NewsBasicResponseDTO>>
    {
        
    }
}