using Domain.entity;
using Domain.repositories;
using MediatR;
using Application.Requests.XuLyNews;
using FluentValidation;
using FluentValidation.Results;

namespace Application.XuLyNews.UseCases;

public class CreateNewsUseCase : IRequestHandler<CreateNewsRequest, bool>
{
    private readonly IMenuRepo menuRepo;
    private readonly INewsRepo newRepo;
    private readonly IWebsiteLocalizationWardRepo wardRepo;
    private readonly IUnitOfWork uow;
    private readonly IWebsiteLocalizationRepo localizationRepo;
    public CreateNewsUseCase(IMenuRepo menusRepo,INewsRepo newsRepo,IWebsiteLocalizationWardRepo wardsRepo,IUnitOfWork uowR,IWebsiteLocalizationRepo lRepo)
    {
        menuRepo = menusRepo;
        newRepo = newsRepo;
        wardRepo = wardsRepo;
        uow = uowR;
        localizationRepo=lRepo;
    }
    public async Task<bool> Handle(CreateNewsRequest request, CancellationToken cancellationToken)
    {
        var existing = await newRepo.GetBySlugAsync(request.Slug.Trim().ToLower());
            if (existing != null)
            {
                throw new ValidationException(new[]{new ValidationFailure(nameof(request.Slug),$"Slug news '{request.Slug}' đã tồn tại.")});
            }
        await uow.BeginTransactionAsync(cancellationToken);
        try
        {
            var wardId = await ResolveWardIdAsync(request.CountryKey,request.ProvinceId,request.WardId,request.Address,cancellationToken);
            var news = new Domain.entity.News
            {
                Title      = request.Title.Trim(),
                Slug       = request.Slug.Trim().ToLower(),
                Content    = request.Content,
                Thumbnail  = request.Thumbnail,
                Address    = string.IsNullOrWhiteSpace(request.Address) ? null : request.Address.Trim(),
                WardId     = wardId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            foreach (var item in request.DanhSachMenus)
            {
                var menu = await menuRepo.GetBySlugAsync(item.Slug.Trim().ToLower());
                if (menu != null)
                {
                    if (!string.Equals(menu.Name?.Trim(), item.Name.Trim(), StringComparison.OrdinalIgnoreCase))
                    {
                        throw new ValidationException(new[]
                        {
                            new ValidationFailure(
                                nameof(item.Slug),
                                $"Slug menu '{item.Slug}' đã tồn tại với tên khác.")
                        });
                    }
                }
                else
                {
                    menu = new Menu
                    {
                        Name       = item.Name.Trim(),
                        Slug       = item.Slug.Trim().ToLower(),
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    await menuRepo.AddAsync(menu);
                }
                news.MenuNews.Add(new MenuNews{Menu=menu});
            }
            await newRepo.AddAsync(news);
            await uow.CommitAsync(cancellationToken);
            return true;
        }
        catch
        {
            await uow.RollbackAsync(cancellationToken);
            throw;
        }
    }
        private async Task<int?> ResolveWardIdAsync(string? countryKey,int? provinceId,int? wardId,string? address,CancellationToken cancellationToken)
        {
            var hasCountry =!string.IsNullOrWhiteSpace(countryKey);
            var hasProvince =provinceId.HasValue;
            var hasWard =wardId.HasValue;
            var hasAddress =!string.IsNullOrWhiteSpace(address);
            if (!hasCountry &&!hasProvince &&!hasWard &&!hasAddress)
            {
                return null;
            }
            if (!hasCountry || !hasProvince || !hasWard || !hasAddress)
            {
                throw new ValidationException(new[]{new ValidationFailure("Address","Phải nhập đầy đủ quốc gia, tỉnh/thành phố, phường/xã và địa chỉ.")});
            }
            var normalizedCountryKey =countryKey!.Trim().ToUpperInvariant();
            var country = await localizationRepo.GetByKeyAsync(normalizedCountryKey);
            if (country == null)
            {
                throw new ValidationException(new[]{ new ValidationFailure(nameof(countryKey),$"Không tìm thấy quốc gia có mã '{countryKey}'.")});
            }
            var province = await wardRepo.GetByIdAsync(provinceId!.Value);
            if (province == null || province.WardPid != 0)
            {
                throw new ValidationException(new[]{new ValidationFailure(nameof(provinceId),$"Không tìm thấy tỉnh/thành phố có Id = {provinceId}.")});
            }
            if (!string.Equals(province.KeyLocalization,normalizedCountryKey,StringComparison.OrdinalIgnoreCase))
            {
                throw new ValidationException(new[]{ new ValidationFailure(nameof(provinceId),$"Tỉnh/thành phố Id = {provinceId} không thuộc quốc gia '{countryKey}'.")});
            }
            var ward = await wardRepo.GetByIdAsync(wardId!.Value);
            if (ward == null)
            {
                throw new ValidationException(new[]{new ValidationFailure(nameof(wardId),$"Không tìm thấy phường/xã có Id = {wardId}.")});
            }
            if (ward.WardPid != province.WardId)
            {
                throw new ValidationException(new[] { new ValidationFailure(nameof(wardId),$"Phường/xã Id = {wardId} không thuộc tỉnh Id = {provinceId}.")});
            }
            if (!string.Equals(ward.KeyLocalization,normalizedCountryKey,StringComparison.OrdinalIgnoreCase))
            {
                throw new ValidationException(new[]
                { new ValidationFailure(nameof(wardId),$"Phường/xã Id = {wardId} không thuộc quốc gia '{countryKey}'.")});
            }
            return ward.WardId;
        }
}