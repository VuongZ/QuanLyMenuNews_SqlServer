using Application.DTO;
using Application.Requests.XuLyAddress;
using Domain.repositories;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Application.Usecase.XuLyAddress;

public class GetAddressByIdUseCase
    : IRequestHandler<GetAddressByIdRequest, WardInfoResponseDto?>
{
    private readonly IWebsiteLocalizationWardRepo wardRepo;

    public GetAddressByIdUseCase(IWebsiteLocalizationWardRepo wardRepo)
    {
        this.wardRepo = wardRepo;
    }

    public async Task<WardInfoResponseDto?> Handle(
        GetAddressByIdRequest request,
        CancellationToken cancellationToken)
    {
        var w = await wardRepo.GetByIdAsync(request.id);

        if (w == null)
        {
            throw new ValidationException(new[]
            {
                new ValidationFailure(nameof(request.id), $"ID address '{request.id}' không tồn tại.")
            });
        }
        return new WardInfoResponseDto
        {
            WardId = w.WardId,
            WardPid = w.WardPid,
            Name = w.Name,
            NameEn = w.NameEn,
            FullName = w.FullName,
            FullNameEn = w.FullNameEn,
            Country = w.Localization?.Localization,
            WardParent = w.WardPid == 0 ? null : new WardParentResponseDto
            {
                WardId = w.WardPid,
                Name = w.FullName != null && w.FullName.Contains(",")? w.FullName.Substring(w.FullName.IndexOf(",") + 1).Trim(): string.Empty,
                NameEn = w.FullNameEn != null && w.FullNameEn.Contains(",")? w.FullNameEn.Substring(w.FullNameEn.IndexOf(",") + 1).Trim(): null,
                Country = w.Localization?.Localization,
            }
        };
    }
}
