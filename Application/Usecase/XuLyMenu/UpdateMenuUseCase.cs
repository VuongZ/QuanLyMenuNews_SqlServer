using Application.Requests.XuLyMenu;
using Domain.entity;
using Domain.repositories;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Application.Usecase.XuLyMenu;

public class UpdateMenuUseCase
    : IRequestHandler<UpdateMenuRequest, bool>
{
    private readonly IMenuRepo menuRepo;
    private readonly INewsRepo newRepo;
    IMenuNewsRepo menuNewRepo;
    IWebsiteLocalizationWardRepo wardRepo;
    private readonly IUnitOfWork uow;

    public UpdateMenuUseCase(IMenuRepo menusRepo, INewsRepo newsRepo, IUnitOfWork uowr, IMenuNewsRepo menuNewsRepo, IWebsiteLocalizationWardRepo wardsRepo)
    {
        menuRepo = menusRepo;
        newRepo = newsRepo;
        uow = uowr;
        menuNewRepo = menuNewsRepo;
        wardRepo = wardsRepo;
    }

    public async Task<bool> Handle(UpdateMenuRequest request, CancellationToken cancellationToken)
    {
      return false;
    }
    private async Task<int?> ResolveWardIdAsync(int? provinceId, int? wardId, string? address)
    {
        var hasProvince = provinceId.HasValue;
        var hasWard     = wardId.HasValue;
        var hasAddress  = !string.IsNullOrWhiteSpace(address);
        if (!hasProvince && !hasWard && !hasAddress)
            return null;
        if (!hasProvince || !hasWard || !hasAddress)
        {
            throw new ValidationException(new[]
            {
                new ValidationFailure(
                    "Address",
                    "Phải nhập đầy đủ tỉnh/thành phố, phường/xã và địa chỉ.")
            });
        }
        var province = await wardRepo.GetByIdAsync(provinceId!.Value);
        if (province == null || province.WardPid != 0)
        {
            throw new ValidationException(new[]
            {
                new ValidationFailure(
                    nameof(provinceId),
                    $"Không tìm thấy tỉnh/thành phố có Id = {provinceId}.")
            });
        }

        var ward = await wardRepo.GetByIdAsync(wardId!.Value);
        if (ward == null)
        {
            throw new ValidationException(new[]
            {
                new ValidationFailure(
                    nameof(wardId),
                    $"Không tìm thấy phường/xã có Id = {wardId}.")
            });
        }
        if (ward.WardPid != provinceId!.Value)
        {
            throw new ValidationException(new[]
            {
                new ValidationFailure(
                    nameof(wardId),
                    $"Phường/xã Id = {wardId} không thuộc tỉnh Id = {provinceId}.")
            });
        }
        return ward.WardId;
    }
}