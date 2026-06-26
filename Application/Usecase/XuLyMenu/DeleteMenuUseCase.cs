using Application.Requests.XuLyMenu;
using Domain.repositories;
using MediatR;

namespace Application.Usecase.XuLyMenu;
public class DeleteMenuUseCase : IRequestHandler<DeleteMenuRequest,bool>
{
       private readonly IMenuRepo _repo;
          private readonly IUnitOfWork uow;
        public DeleteMenuUseCase(IMenuRepo repo,IUnitOfWork uow)
        {
            _repo = repo;
            uow = uow;
        }

    public async Task<bool> Handle(DeleteMenuRequest request, CancellationToken cancellationToken)
    {
          var menu = await _repo.GetByIdAsync(request.Id);
            if (menu == null) return false;

          await _repo.DeleteAsync(request.Id);
         await uow.SaveChangesAsync(cancellationToken);

          return true;
          
    }
}