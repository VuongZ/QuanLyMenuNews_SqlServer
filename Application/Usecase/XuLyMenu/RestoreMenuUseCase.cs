using Application.XuLyMenu.Requests;
using Domain.repositories;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Application.XuLyMenu.UseCases;
public class RestoreMenuUseCase : IRequestHandler<RestoreMenuRequest, bool>
{
    private readonly IMenuRepo menuRepo;
    private readonly IUnitOfWork uow;

    public RestoreMenuUseCase(IMenuRepo menuRepo, IUnitOfWork uow)
    {
        this.menuRepo = menuRepo;
        this.uow = uow;
    }

    public async Task<bool> Handle(RestoreMenuRequest request, CancellationToken cancellationToken)
    {
        var exists = await menuRepo.ExistsDeletedAsync(request.Id);
        if (!exists)
        {
            throw new ValidationException(new[] { new ValidationFailure(nameof(request.Id), "Menu không tồn tại hoặc chưa bị xóa.") });
        }

        await uow.BeginTransactionAsync(cancellationToken);
        try
        {
            await menuRepo.RestoreAsync(request.Id);
            await uow.CommitAsync(cancellationToken);
            return true;
        }
        catch
        {
            await uow.RollbackAsync(cancellationToken);
            throw;
        }
    }
}