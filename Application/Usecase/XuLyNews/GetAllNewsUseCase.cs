using Application.DTO;
using Application.XuLyNews.Requests;
using Domain.entity;
using Domain.repositories;
using MediatR;

namespace Application.XuLyNews.UsesCases
{
    
    public class GetAllNewsUseCase : IRequestHandler<GetAllNewsRequest, IEnumerable<NewsResponseDto>>
    {
        private readonly INewsRepo _newsRepo;

        public GetAllNewsUseCase(INewsRepo newsRepo)
        {
            _newsRepo = newsRepo;
        }

        public async Task<IEnumerable<NewsResponseDto>> Handle(GetAllNewsRequest request, CancellationToken cancellationToken)
        {
            var news = await _newsRepo.GetAllWithMenusAsync();
            return news.Select(n => new NewsResponseDto
            {
                Id = n.Id,
                Title = n.Title ?? string.Empty,
                Slug = n.Slug ?? string.Empty,
                Content = n.Content ?? string.Empty,
                Thumbnail = n.thumbnail ?? string.Empty,

                Menus = n.Menu.Select(m => new MenuResponseDto
                {
                    Id = m.Id,
                    Name = m.Name ?? string.Empty,
                    Slug = m.Slug ?? string.Empty,
                }).ToList()
            });
        }
    }
}