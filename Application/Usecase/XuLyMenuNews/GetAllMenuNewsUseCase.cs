using Application.DTO;
using Application.Requests.XuLyMenuNews;
using Domain.repositories;
using MediatR;

namespace Application.Usecase.XuLyMenuNews
{
    public class GetAllMenuNewsUseCase : IRequestHandler<GetAllMenuNewsRequest, IEnumerable<MenuNewsResponseDto>>
    {
        private readonly IMenuNewsRepo _menuNewsRepo;

        public GetAllMenuNewsUseCase(IMenuNewsRepo menuNewsRepo)
        {
            _menuNewsRepo = menuNewsRepo;
        }

        public async Task<IEnumerable<MenuNewsResponseDto>> Handle(GetAllMenuNewsRequest request, CancellationToken cancellationToken)
        {
            var menuNews = await _menuNewsRepo.GetAllAsync();
            return menuNews.Select(MenuNewsMapper.ToDto);
        }
    }
}
