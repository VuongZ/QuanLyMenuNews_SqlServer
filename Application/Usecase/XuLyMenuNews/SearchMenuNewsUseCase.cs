using Application.DTO;
using Application.Requests.XuLyMenuNews;
using Domain.repositories;
using MediatR;

namespace Application.Usecase.XuLyMenuNews
{
    public class SearchMenuNewsUseCase : IRequestHandler<SearchMenuNewsRequest, IEnumerable<MenuNewsResponseDto>>
    {
        private readonly IMenuNewsRepo menuNewRepo;
        public SearchMenuNewsUseCase(IMenuNewsRepo menuNewsRepo)
        {
            menuNewRepo = menuNewsRepo;
        }
        public Task<IEnumerable<MenuNewsResponseDto>> Handle(SearchMenuNewsRequest request, CancellationToken cancellationToken)
        {
            var result = menuNewRepo.SearchAsync(request.MenuId, request.NewsId)
                .Select(menuNews => new MenuNewsResponseDto
                {
                    MenuId = menuNews.MenuId,
                    NewsId = menuNews.NewsId,
                    MenuName = menuNews.Menu == null ? string.Empty : menuNews.Menu.Name ?? string.Empty,
                    NewsTitle = menuNews.News == null ? string.Empty : menuNews.News.Title ?? string.Empty
                })
                .AsEnumerable();
            return Task.FromResult(result);
        }
    }
}
