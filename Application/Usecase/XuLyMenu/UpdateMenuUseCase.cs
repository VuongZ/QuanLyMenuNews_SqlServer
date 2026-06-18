using Application.Requests.XuLyMenu;
using Domain.repositories;
using FluentValidation;
using MediatR;

namespace Application.Usecase.XuLyMenu
{
    public class UpdateMenuUseCase : IRequestHandler<UpdateMenuRequest, bool>
    {
        private readonly IMenuRepo _repo;
          private readonly IUnitOfWork _uow;
        public UpdateMenuUseCase(IMenuRepo repo,IUnitOfWork uow)
        {
            _repo = repo;
                _uow = uow;
        }
        public async Task<bool> Handle(UpdateMenuRequest request, CancellationToken cancellationToken)
        {
            var existing = await _repo.GetBySlugAsync(request.Slug.Trim().ToLower());

             if (existing != null && existing.Id != request.id)
          {
            throw new ValidationException("Slug menu đã tồn tại.");
             }
                
           var menu = await _repo.GetByIdAsync(request.id);
            if (menu == null) return false;

            menu.Name = request.Name;
            menu.Slug = request.Slug.Trim().ToLower();
            menu.updated_at = DateTime.Now;

            await _repo.UpdateAsync(menu);
            await _uow.SaveChangesAsync(cancellationToken);
            return true;
          
        }
    }
}