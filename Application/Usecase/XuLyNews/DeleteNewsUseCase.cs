using Application.Requests.XuLyNews;
using Domain.entity;
using Domain.repositories;
using MediatR;

namespace Application.Usecase.XuLyNews
{
    public class DeleteNewsUseCase : IRequestHandler<DeleteNewsRequest, bool>
    {
        private readonly INewsRepo newsRepo;
                private readonly IUnitOfWork _uow;
        public DeleteNewsUseCase(INewsRepo Repo, IUnitOfWork uow)
        {
            newsRepo = Repo;
            _uow = uow;
        }
        public async Task<bool> Handle(DeleteNewsRequest request, CancellationToken cancellationToken)
        {
            var news = await newsRepo.GetByIdAsync(request.id);
            if (news == null)
                return false;

            await newsRepo.DeleteAsync(request.id);
            await _uow.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}