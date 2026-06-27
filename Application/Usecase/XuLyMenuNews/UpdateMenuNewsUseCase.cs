using Application.Requests.XuLyMenuNews;
using Domain.entity;
using Domain.repositories;
using FluentValidation;
using MediatR;

namespace Application.Usecase.XuLyMenuNews
{
    public class UpdateMenuNewsUseCase : IRequestHandler<UpdateMenuNewsRequest, bool>
    {
        private readonly IMenuNewsRepo menuNewsRepo;
        private readonly IMenuRepo menuRepo;
        private readonly INewsRepo newsRepo;
        private readonly IUnitOfWork uow;

        public UpdateMenuNewsUseCase(
            IMenuNewsRepo menuNewsRepo,
            IMenuRepo menuRepo,
            INewsRepo newsRepo,
            IUnitOfWork uow)
        {
            this.menuNewsRepo = menuNewsRepo;
            this.menuRepo = menuRepo;
            this.newsRepo = newsRepo;
            this.uow = uow;
        }

        public async Task<bool> Handle(UpdateMenuNewsRequest request, CancellationToken cancellationToken)
        {
            var oldMenuNews = menuNewsRepo.GetByIdAsync(request.OldMenuId, request.OldNewsId).FirstOrDefault();
            if (oldMenuNews == null)
            {
                return false;
            }

            var menu = await menuRepo.GetByIdAsync(request.MenuId);
            if (menu == null)
            {
                throw new ValidationException("Menu không tồn tại.");
            }

            var news = await newsRepo.GetByIdAsync(request.NewsId);
            if (news == null)
            {
                throw new ValidationException("News không tồn tại.");
            }

            var existing = menuNewsRepo.GetByIdAsync(request.MenuId, request.NewsId).FirstOrDefault();
            if (existing != null && (request.MenuId != request.OldMenuId || request.NewsId != request.OldNewsId))
            {
                throw new ValidationException("Menu_News đã tồn tại.");
            }

            if (request.MenuId == request.OldMenuId && request.NewsId == request.OldNewsId)
            {
                return true;
            }

            await menuNewsRepo.DeleteAsync(request.OldMenuId, request.OldNewsId);
            await menuNewsRepo.AddAsync(new MenuNews
            {
                MenuId = request.MenuId,
                NewsId = request.NewsId
            });
            await uow.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
