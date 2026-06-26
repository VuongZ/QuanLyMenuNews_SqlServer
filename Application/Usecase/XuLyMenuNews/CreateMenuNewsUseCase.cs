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
        private readonly IMenuNewsRepo menuNewsRepo;
        private readonly IMenuRepo menuRepo;
        private readonly INewsRepo newsRepo;
        private readonly IUnitOfWork uow;

        public CreateMenuNewsUseCase(
            IMenuNewsRepo menuNewsRepo,
            IMenuRepo menuRepo,
            INewsRepo newsRepo,
            IUnitOfWork uow)
        {
            menuNewsRepo = menuNewsRepo;
            menuRepo = menuRepo;
            newsRepo = newsRepo;
            uow = uow;
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

    if (await menuNewsRepo.ExistsAsync(
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
