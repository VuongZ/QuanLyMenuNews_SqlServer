using Application.DTO;
using Application.Requests.XuLyMenuNews;
using Domain.repositories;
using MediatR;

namespace Application.Usecase.XuLyMenuNews
{
    public class GetAllMenuNewsUseCase : IRequestHandler<GetAllMenuNewsRequest, IEnumerable<MenuNewsResponseDto>>
    {
        private readonly IMenuNewsRepo menuNewsRepo;
        public GetAllMenuNewsUseCase(IMenuNewsRepo menuNewsRepo)
        {
            this.menuNewsRepo = menuNewsRepo;
        }
        public Task<IEnumerable<MenuNewsResponseDto>> Handle(GetAllMenuNewsRequest request, CancellationToken cancellationToken)
        {
            var result = menuNewsRepo.GetAllAsync()
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
