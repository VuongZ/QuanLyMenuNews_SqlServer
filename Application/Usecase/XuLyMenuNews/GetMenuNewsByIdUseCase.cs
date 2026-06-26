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
            menuNewsRepo = menuNewsRepo;
        }

        public async Task<MenuNewsResponseDto?> Handle(GetMenuNewsByIdRequest request, CancellationToken cancellationToken)
        {
            var menuNews = await menuNewsRepo.GetByIdAsync(request.MenuId, request.NewsId);
            return menuNews == null ? null : MenuNewsMapper.ToDto(menuNews);
        }
    }
}
