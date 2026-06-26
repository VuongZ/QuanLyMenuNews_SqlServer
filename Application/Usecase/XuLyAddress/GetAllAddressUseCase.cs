using Application.DTO;
using Application.Requests.XuLyAddress;
using Domain.repositories;
using MediatR;

namespace Application.Usecase.XuLyAddress;

public class GetAllAddressUseCase
    : IRequestHandler<GetAllAddressRequest, IEnumerable<WardInfoResponseDto>>
{
    private readonly IWebsiteLocalizationWardRepo wardRepo;

    public GetAllAddressUseCase(IWebsiteLocalizationWardRepo wardRepo)
    {
        this.wardRepo = wardRepo;
    }

    public async Task<IEnumerable<WardInfoResponseDto>> Handle(
        GetAllAddressRequest request,
        CancellationToken cancellationToken)
    {
        return (await wardRepo.GetAllAsync())
            .Select(w => new WardInfoResponseDto
            {
                WardId = w.WardId,
                WardPid = w.WardPid,
                Name = w.Name,
                NameEn = w.NameEn,
                FullName = w.FullName,
                FullNameEn = w.FullNameEn,
                Country = w.Localization?.Localization ?? string.Empty,
                WardParent = w.WardPid == 0 ? null : new WardParentResponseDto
                {
                    WardId = w.WardPid,
                    Name = w.FullName != null && w.FullName.Contains(",")? w.FullName.Substring(w.FullName.IndexOf(",") + 1).Trim(): string.Empty,
                    NameEn = w.FullNameEn != null && w.FullNameEn.Contains(",")? w.FullNameEn.Substring(w.FullNameEn.IndexOf(",") + 1).Trim(): null,
                    Country = w.Localization?.Localization ?? string.Empty,
                }
            })
            .AsEnumerable();
    }
}
