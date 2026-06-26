using Application.Requests.XuLyMenuNews;
using Domain.repositories;
using MediatR;

namespace Application.Usecase.XuLyMenuNews
{
    public class DeleteMenuNewsUseCase : IRequestHandler<DeleteMenuNewsRequest, bool>
    {
        private readonly IMenuNewsRepo menuNewsRepo;
        private readonly IUnitOfWork uow;

        public DeleteMenuNewsUseCase(IMenuNewsRepo menuNewsRepo, IUnitOfWork uow)
        {
            menuNewsRepo = menuNewsRepo;
            uow = uow;
        }

        public async Task<bool> Handle(DeleteMenuNewsRequest request, CancellationToken cancellationToken)
        {
            var menuNews = await menuNewsRepo.GetByIdAsync(request.MenuId, request.NewsId);
            if (menuNews == null)
            {
                return false;
            }

            await menuNewsRepo.DeleteAsync(request.MenuId, request.NewsId);
            await uow.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
