using Application.Requests.XuLyMenu;
using Domain.repositories;
using MediatR;

namespace Application.Usecase.XuLyMenu;
public class DeleteMenuUseCase : IRequestHandler<DeleteMenuRequest,bool>
{
        private readonly IMenuRepo menurepo;
        private readonly IUnitOfWork uow;
        public DeleteMenuUseCase(IMenuRepo repo,IUnitOfWork uows)
        {
            menurepo = repo;
            uow = uows;
        }

    public async Task<bool> Handle(DeleteMenuRequest request, CancellationToken cancellationToken)
    {
          var menu = await menurepo.GetByIdAsync(request.Id);
            if (menu == null) return false;

          await menurepo.DeleteAsync(request.Id);
         await uow.SaveChangesAsync(cancellationToken);

          return true;
          
    }
}