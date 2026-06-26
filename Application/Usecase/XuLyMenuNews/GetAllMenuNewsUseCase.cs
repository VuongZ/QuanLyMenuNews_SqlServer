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
            menuNewsRepo = menuNewsRepo;
        }

        public async Task<IEnumerable<MenuNewsResponseDto>> Handle(GetAllMenuNewsRequest request, CancellationToken cancellationToken)
        {
            var menuNews = await menuNewsRepo.GetAllAsync();
            return menuNews.Select(MenuNewsMapper.ToDto);
        }
    }
}
