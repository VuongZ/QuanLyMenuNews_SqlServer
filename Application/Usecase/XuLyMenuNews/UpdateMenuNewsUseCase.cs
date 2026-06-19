using Application.Requests.XuLyMenuNews;
using Domain.entity;
using Domain.repositories;
using FluentValidation;
using MediatR;

namespace Application.Usecase.XuLyMenuNews
{
    public class UpdateMenuNewsUseCase : IRequestHandler<UpdateMenuNewsRequest, bool>
    {
        private readonly IMenuNewsRepo _menuNewsRepo;
        private readonly IMenuRepo _menuRepo;
        private readonly INewsRepo _newsRepo;
        private readonly IUnitOfWork _uow;

        public UpdateMenuNewsUseCase(
            IMenuNewsRepo menuNewsRepo,
            IMenuRepo menuRepo,
            INewsRepo newsRepo,
            IUnitOfWork uow)
        {
            _menuNewsRepo = menuNewsRepo;
            _menuRepo = menuRepo;
            _newsRepo = newsRepo;
            _uow = uow;
        }

        public async Task<bool> Handle(UpdateMenuNewsRequest request, CancellationToken cancellationToken)
        {
            var oldMenuNews = await _menuNewsRepo.GetByIdAsync(request.OldMenuId, request.OldNewsId);
            if (oldMenuNews == null)
            {
                return false;
            }

            var menu = await _menuRepo.GetByIdAsync(request.MenuId);
            if (menu == null)
            {
                throw new ValidationException("Menu không tồn tại.");
            }

            var news = await _newsRepo.GetByIdAsync(request.NewsId);
            if (news == null)
            {
                throw new ValidationException("News không tồn tại.");
            }

            var existing = await _menuNewsRepo.GetByIdAsync(request.MenuId, request.NewsId);
            if (existing != null && (request.MenuId != request.OldMenuId || request.NewsId != request.OldNewsId))
            {
                throw new ValidationException("Menu_News đã tồn tại.");
            }

            if (request.MenuId == request.OldMenuId && request.NewsId == request.OldNewsId)
            {
                return true;
            }

            await _menuNewsRepo.DeleteAsync(request.OldMenuId, request.OldNewsId);
            await _menuNewsRepo.AddAsync(new MenuNews
            {
                MenuId = request.MenuId,
                NewsId = request.NewsId
            });
            await _uow.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
