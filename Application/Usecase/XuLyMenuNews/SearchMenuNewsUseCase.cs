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

        public async Task<IEnumerable<MenuNewsResponseDto>> Handle(SearchMenuNewsRequest request, CancellationToken cancellationToken)
        {
            var menuNews = await menuNewRepo.SearchAsync(request.MenuId, request.NewsId);
            return menuNews.Select(MenuNewsMapper.ToDto);
        }
    }
}
