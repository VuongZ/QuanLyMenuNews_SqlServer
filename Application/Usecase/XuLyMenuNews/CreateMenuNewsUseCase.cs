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
        private readonly IMenuRepo menuRepo;
        private readonly INewsRepo newsRepo;
        private readonly IUnitOfWork _uow;

        public CreateMenuNewsUseCase(
            IMenuNewsRepo menuNewsRepo,
            IMenuRepo menuRepo,
            INewsRepo newsRepo,
            IUnitOfWork uow)
        {
            _menuNewsRepo = menuNewsRepo;
            menuRepo = menuRepo;
            newsRepo = newsRepo;
            _uow = uow;
        }

     public async Task<bool> Handle(CreateMenuNewsRequest request, CancellationToken cancellationToken){
    if (!await menuRepo.ExistsAsync(request.MenuId))
    {
        throw new ValidationException(new[]
        {
            new ValidationFailure(
                nameof(request.MenuId),
                $"Không tìm thấy Menu có Id = {request.MenuId}."
            )
        });
    }

    if (!await newsRepo.ExistsAsync(request.NewsId))
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
