using Application.DTO;
using Application.Requests.XuLyNews;
using Domain.entity;
using Domain.repositories;
using MediatR;

namespace Application.Usecase.XuLyNews
{
    public class GetNewsByIdUseCase : IRequestHandler<GetNewsByIdRequest, NewsResponseDto?>
    {
        private readonly INewsRepo _newsRepo;
        public GetNewsByIdUseCase(INewsRepo newsRepo)
        {
            _newsRepo = newsRepo;
        }
        public async Task<NewsResponseDto?> Handle(GetNewsByIdRequest request, CancellationToken cancellationToken)
        {

            var news = await _newsRepo.GetByIdWithMenusAsync(request.id);
            if (news != null)            {
                return new NewsResponseDto
            {
                Id = news.Id,
                Title = news.Title ?? string.Empty,
                Slug = news.Slug ?? string.Empty,
                Content = news.Content ?? string.Empty,
                Thumbnail = news.thumbnail ?? string.Empty,
                Menus = news.Menu.Select(m => new MenuResponseDto
                {
                    Id = m.Id,
                    Name = m.Name ?? string.Empty,
                    Slug = m.Slug ?? string.Empty,
                }).ToList()
            };
            }
            return null;
          
    }
}
}