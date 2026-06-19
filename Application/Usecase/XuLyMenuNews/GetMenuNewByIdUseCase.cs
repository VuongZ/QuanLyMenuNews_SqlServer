using Application.DTO;
using Application.Requests.XuLyMenuNews;
using Domain.repositories;
using MediatR;

namespace Application.Usecase.XuLyMenuNews
{
    public class SearchMenuNewsUseCase : IRequestHandler<SearchMenuNewsRequest, IEnumerable<MenuNewsResponseDto>>
    {
        private readonly IMenuNewsRepo _menuNewsRepo;

        public SearchMenuNewsUseCase(IMenuNewsRepo menuNewsRepo)
        {
            _menuNewsRepo = menuNewsRepo;
        }

        public async Task<IEnumerable<MenuNewsResponseDto>> Handle(SearchMenuNewsRequest request, CancellationToken cancellationToken)
        {
            var menuNews = await _menuNewsRepo.GetById(request.MenuId, request.NewsId);
            return menuNews.Select(MenuNewsMapper.ToDto);
        }
    }
}
