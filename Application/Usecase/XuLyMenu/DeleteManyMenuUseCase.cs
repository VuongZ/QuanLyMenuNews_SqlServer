using Application.Requests.XuLyMenu;
using Domain.repositories;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Application.Usecase.XuLyMenu;

public class DeleteManyMenuUseCase
    : IRequestHandler<DeleteManyMenuRequest, int>
{
    private readonly IMenuRepo _menuRepo;
    private readonly IUnitOfWork _uow;

    public DeleteManyMenuUseCase(
        IMenuRepo menuRepo,
        IUnitOfWork uow)
    {
        _menuRepo = menuRepo;
        _uow = uow;
    }

   public async Task<int> Handle(
    DeleteManyMenuRequest request,
    CancellationToken cancellationToken)
{
    await _uow.BeginTransactionAsync(cancellationToken);

    try
    {
        var ids = request.Ids.Distinct().ToList();
        var deletedCount =await _menuRepo.SoftDeleteManyAsync(ids);
        if (deletedCount != ids.Count)
        {
            throw new ValidationException(new[]
            {
                new ValidationFailure(
                    nameof(request.Ids),
                    "Có Menu không tồn tại hoặc đã bị xóa."
                )
            });
        }
        await _uow.CommitAsync(cancellationToken);
        return deletedCount;
    }
    catch
    {
        await _uow.RollbackAsync(cancellationToken);
        throw;
    }
}
}