using Application.DTO;
using Domain.entity;
using MediatR;

namespace Application.Requests.XuLyNews
{
    public class GetNewsByIdRequest : IRequest<NewsResponseDto>
    {
        public int id { get; set; }
    }
}