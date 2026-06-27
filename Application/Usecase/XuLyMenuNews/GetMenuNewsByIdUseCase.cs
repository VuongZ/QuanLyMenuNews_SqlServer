using Application.DTO;
using Application.Requests.XuLyMenuNews;
using Domain.repositories;
using MediatR;

namespace Application.Usecase.XuLyMenuNews
{
    public class GetMenuNewsByIdUseCase : IRequestHandler<GetMenuNewsByIdRequest, MenuNewsResponseDto?>
    {
        private readonly IMenuNewsRepo menuNewsRepo;

        public GetMenuNewsByIdUseCase(IMenuNewsRepo menuNewsRepo)
        {
            this.menuNewsRepo = menuNewsRepo;
        }

        public Task<MenuNewsResponseDto?> Handle(GetMenuNewsByIdRequest request, CancellationToken cancellationToken)
        {
            var result = menuNewsRepo.GetByIdAsync(request.MenuId, request.NewsId)
                .Select(menuNews => new MenuNewsResponseDto
                {
                    MenuId = menuNews.MenuId,
                    NewsId = menuNews.NewsId,
                    MenuName = menuNews.Menu == null ? string.Empty : menuNews.Menu.Name ?? string.Empty,
                    NewsTitle = menuNews.News == null ? string.Empty : menuNews.News.Title ?? string.Empty
                })
                .FirstOrDefault();

            return Task.FromResult(result);
        }
    }
}
