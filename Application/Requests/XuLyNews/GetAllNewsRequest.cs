using Application.DTO;
using Domain.entity;
using MediatR;

namespace Application.XuLyNews.Requests
{
    public class GetAllNewsRequest : IRequest<IEnumerable<NewsResponseDto>>
    {
           public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}