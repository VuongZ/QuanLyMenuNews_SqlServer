using Application.Requests.XuLyMenuNews;
using Domain.entity;
using Domain.repositories;
using FluentValidation;
using FluentValidation.Results;
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

     public async Task<bool> Handle(CreateMenuNewsRequest request, CancellationToken cancellationToken){
    if (!await _menuRepo.ExistsAsync(request.MenuId))
    {
        throw new ValidationException(new[]
        {
            new ValidationFailure(
                nameof(request.MenuId),
                $"Không tìm thấy Menu có Id = {request.MenuId}."
            )
        });
    }

    if (!await _newsRepo.ExistsAsync(request.NewsId))
    {
        throw new ValidationException(new[]
        {
            new ValidationFailure(
                nameof(request.NewsId),
                $"Không tìm thấy News có Id = {request.NewsId}."
            )
        });
    }

    if (await _menuNewsRepo.ExistsAsync(
        request.MenuId,
        request.NewsId))
    {
        throw new ValidationException(new[]
        {
            new ValidationFailure(
                "MenuNews",
                "Quan hệ giữa Menu và News đã tồn tại."
            )
        });
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
