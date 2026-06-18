using Domain.entity;
using MediatR;

namespace Application.Requests.XuLyNews
{
    public class DeleteNewsRequest : IRequest<bool>
    {
        public int id { get; set; }
    }
}