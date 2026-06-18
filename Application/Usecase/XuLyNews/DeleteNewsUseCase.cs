using Application.Requests.XuLyNews;
using Domain.entity;
using Domain.repositories;
using MediatR;

namespace Application.Usecase.XuLyNews
{
    public class DeleteNewsUseCase : IRequestHandler<DeleteNewsRequest, bool>
    {
        private readonly INewsRepo _newsRepo;
                private readonly IUnitOfWork _uow;
        public DeleteNewsUseCase(INewsRepo newsRepo, IUnitOfWork uow)
        {
            _newsRepo = newsRepo;
            _uow = uow;
        }
        public async Task<bool> Handle(DeleteNewsRequest request, CancellationToken cancellationToken)
        {
            var news = await _newsRepo.GetByIdAsync(request.id);
            if (news == null)
                return false;

            await _newsRepo.DeleteAsync(request.id);
            await _uow.SaveChangesAsync();
            return true;
        }
    }
}