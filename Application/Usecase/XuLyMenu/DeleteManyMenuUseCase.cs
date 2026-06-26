using Application.Requests.XuLyMenu;
using Domain.repositories;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Application.Usecase.XuLyMenu;

public class DeleteManyMenuUseCase
    : IRequestHandler<DeleteManyMenuRequest, int>
{
    private readonly IMenuRepo menuRepo;
    private readonly IUnitOfWork uow;

    public DeleteManyMenuUseCase(
        IMenuRepo menuRepo,
        IUnitOfWork uow)
    {
        menuRepo = menuRepo;
        uow = uow;
    }

   public async Task<int> Handle(
    DeleteManyMenuRequest request,
    CancellationToken cancellationToken)
{
    await uow.BeginTransactionAsync(cancellationToken);

    try
    {
        var ids = request.Ids.Distinct().ToList();
        var deletedCount =await menuRepo.SoftDeleteManyAsync(ids);
        if (deletedCount != ids.Count)
        {
            throw new ValidationException(new[]{new ValidationFailure(nameof(request.Ids), "Có Menu không tồn tại hoặc đã bị xóa.")});
        }
        await uow.CommitAsync(cancellationToken);
        return deletedCount;
    }
    catch
    {
        await uow.RollbackAsync(cancellationToken);
        throw;
    }
}
}