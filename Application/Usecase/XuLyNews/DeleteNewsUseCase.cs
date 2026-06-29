using Application.Requests.XuLyNews;
using Domain.repositories;
using MediatR;

namespace Application.Usecase.XuLyNews
{
    public class DeleteNewsUseCase : IRequestHandler<DeleteNewsRequest, bool>
    {
        private readonly INewsRepo newsRepo;
                private readonly IUnitOfWork uow;
        public DeleteNewsUseCase(INewsRepo Repo, IUnitOfWork Iuow)
        {
            newsRepo = Repo;
            uow = Iuow;
        }
        public async Task<bool> Handle(DeleteNewsRequest request, CancellationToken cancellationToken)
        {
            var news = await newsRepo.GetByIdAsync(request.id);
            if (news == null)
                return false;
            await newsRepo.DeleteAsync(request.id);
            await uow.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}