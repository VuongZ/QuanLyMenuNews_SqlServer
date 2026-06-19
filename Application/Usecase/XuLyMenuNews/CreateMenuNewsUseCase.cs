using Application.Requests.XuLyMenuNews;
using Domain.entity;
using Domain.repositories;
using FluentValidation;
using MediatR;

namespace Application.Usecase.XuLyMenuNews
{
    public class CreateMenuNewsUseCase : IRequestHandler<CreateMenuNewsRequest, bool>
    {
        private readonly IMenuNewsRepo _menuNewsRepo;
        private readonly IMenuRepo _menuRepo;
        private readonly INewsRepo _newsRepo;
        private readonly IUnitOfWork _uow;

        public CreateMenuNewsUseCase(
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

        public async Task<bool> Handle(CreateMenuNewsRequest request, CancellationToken cancellationToken)
        {
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
            if (existing != null)
            {
                throw new ValidationException("Menu_News đã tồn tại.");
            }

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
