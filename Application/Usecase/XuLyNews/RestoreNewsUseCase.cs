using Application.XuLyNews.Requests;
using Domain.repositories;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Application.XuLyNews.UseCases;
public class RestoreNewsUseCase : IRequestHandler<RestoreNewsRequest, bool>
{
    private readonly INewsRepo newsRepo;
    private readonly IUnitOfWork uow;

    public RestoreNewsUseCase(INewsRepo newsRepo, IUnitOfWork uow)
    {
        this.newsRepo = newsRepo;
        this.uow = uow;
    }

    public async Task<bool> Handle(RestoreNewsRequest request, CancellationToken cancellationToken)
    {
        var exists = await newsRepo.ExistsDeletedAsync(request.Id);
        if (!exists)
        {
            throw new ValidationException(new[] { new ValidationFailure(nameof(request.Id), "News không tồn tại hoặc chưa bị xóa.") });
        }

        await uow.BeginTransactionAsync(cancellationToken);
        try
        {
            await newsRepo.RestoreAsync(request.Id);
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