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

    public DeleteManyMenuUseCase(IMenuRepo menusRepo,IUnitOfWork uows)
    {
        menuRepo = menusRepo;
        uow = uows;
    }

    public async Task<int> Handle(DeleteManyMenuRequest request,CancellationToken cancellationToken)
    {
        var ids = request.Ids.Distinct().ToList();
        var existingCount  = await menuRepo.CountByIdsAsync(ids);
            if (existingCount  != ids.Count)
            {
                throw new ValidationException(new[] { new ValidationFailure(nameof(request.Ids), "Có Menu không tồn tại hoặc đã bị xóa."  )});
            }
        await uow.BeginTransactionAsync(cancellationToken);
        try
        {
            var deletedCount = await menuRepo.SoftDeleteManyAsync(ids);
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