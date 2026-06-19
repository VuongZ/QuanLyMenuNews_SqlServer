using Application.Requests.XuLyMenuNews;
using Domain.repositories;
using MediatR;

namespace Application.Usecase.XuLyMenuNews
{
    public class DeleteMenuNewsUseCase : IRequestHandler<DeleteMenuNewsRequest, bool>
    {
        private readonly IMenuNewsRepo _menuNewsRepo;
        private readonly IUnitOfWork _uow;

        public DeleteMenuNewsUseCase(IMenuNewsRepo menuNewsRepo, IUnitOfWork uow)
        {
            _menuNewsRepo = menuNewsRepo;
            _uow = uow;
        }

        public async Task<bool> Handle(DeleteMenuNewsRequest request, CancellationToken cancellationToken)
        {
            var menuNews = await _menuNewsRepo.GetByIdAsync(request.MenuId, request.NewsId);
            if (menuNews == null)
            {
                return false;
            }

            await _menuNewsRepo.DeleteAsync(request.MenuId, request.NewsId);
            await _uow.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
