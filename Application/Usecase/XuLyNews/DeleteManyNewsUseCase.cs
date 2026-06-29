using Application.Requests.XuLyNews;
using Domain.repositories;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Application.Usecase.XuLyNews;

public class DeleteManyNewsUseCase : IRequestHandler<DeleteManyNewsRequest, int>
{
    private readonly INewsRepo newsRepo;
    private readonly IUnitOfWork uow;
    public DeleteManyNewsUseCase(INewsRepo InewsRepo,IUnitOfWork Iuow)
    {
        newsRepo = InewsRepo;
        uow = Iuow;
    }

    public async Task<int> Handle( DeleteManyNewsRequest request,CancellationToken cancellationToken)
    {
        var ids = request.Ids.Distinct().ToList();
        var existingCount  = await newsRepo.CountByIdsAsync(ids);
            if (existingCount  != ids.Count)
            {
                throw new ValidationException(new[] { new ValidationFailure(nameof(request.Ids), "Có News không tồn tại hoặc đã bị xóa."  )});
            }
        await uow.BeginTransactionAsync(cancellationToken);
        try
        {
            var deletedCount = await newsRepo.SoftDeleteManyAsync(ids);
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