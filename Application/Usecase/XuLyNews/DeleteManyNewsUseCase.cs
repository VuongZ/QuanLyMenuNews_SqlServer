using Application.Requests.XuLyNews;
using Domain.repositories;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Application.Usecase.XuLyNews;

public class DeleteManyNewsUseCase : IRequestHandler<DeleteManyNewsRequest, int>
{
    private readonly INewsRepo newsRepo;
    private readonly IUnitOfWork _uow;
    public DeleteManyNewsUseCase(INewsRepo newsRepo,IUnitOfWork uow)
    {
        newsRepo = newsRepo;
        _uow = uow;
    }

    public async Task<int> Handle( DeleteManyNewsRequest request,CancellationToken cancellationToken)
    {
        await _uow.BeginTransactionAsync(cancellationToken);
        try
        {
            var ids = request.Ids
                .Distinct()
                .ToList();
            var deletedCount =
                await newsRepo.SoftDeleteManyAsync(ids);
            if (deletedCount != ids.Count)
            {
                throw new ValidationException(new[]
                {
                    new ValidationFailure(
                        nameof(request.Ids),
                        "Có News không tồn tại hoặc đã bị xóa."
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