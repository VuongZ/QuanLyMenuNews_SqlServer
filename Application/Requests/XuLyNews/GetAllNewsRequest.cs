using Application.DTO;
using Domain.entity;
using MediatR;

namespace Application.XuLyNews.Requests
{
    public class GetAllNewsRequest : IRequest<IEnumerable<NewsResponseDto>>
    {
    }
}