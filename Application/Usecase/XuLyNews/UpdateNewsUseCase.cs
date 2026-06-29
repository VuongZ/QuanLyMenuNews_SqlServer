using AutoMapper;
using Domain.repositories;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Application.Requests.XuLyNews;

public class UpdateNewsUseCase : IRequestHandler<UpdateNewsRequest, bool>
{
    private readonly INewsRepo newsRepo;
    private readonly IMenuRepo menuRepo;
    private readonly IMenuNewsRepo menuNewsRepo;
    private readonly IWebsiteLocalizationWardRepo wardRepo;
    private readonly IWebsiteLocalizationRepo localizationRepo;
    private readonly IUnitOfWork uow;
    private readonly IMapper mapper;
    public UpdateNewsUseCase(INewsRepo nRepo,IMenuRepo mRepo,IMenuNewsRepo mnRepo,IWebsiteLocalizationWardRepo wRepo,IWebsiteLocalizationRepo lRepo,IUnitOfWork uowR,IMapper imapper)
    {
        newsRepo         = nRepo;
        menuRepo         = mRepo;
        menuNewsRepo     = mnRepo;
        wardRepo         = wRepo;
        localizationRepo = lRepo;
        uow              = uowR;
        mapper           = imapper;
    }
    public async Task<bool> Handle(UpdateNewsRequest request, CancellationToken cancellationToken)
    {
        var news = await newsRepo.GetByIdAsync(request.id);
        if (news == null)throw new ValidationException(new[]{new ValidationFailure(nameof(request.id), $"News có Id = {request.id} không tồn tại.")});
        var normalizedNewsSlug = request.slug.Trim().ToLowerInvariant();
        var duplicateNews = await newsRepo.GetBySlugAsync(normalizedNewsSlug);
        if (duplicateNews != null && duplicateNews.Id != request.id)throw new ValidationException(new[]{new ValidationFailure(nameof(request.slug), $"Slug News '{request.slug}' đã tồn tại.")});
        await uow.BeginTransactionAsync(cancellationToken);
        try
        {           
            var resolvedWardId = await ResolveWardIdAsync(request.CountryKey,request.ProvinceId, request.WardId, request.Address,cancellationToken);
            mapper.Map(request, news);
            news.WardId    = resolvedWardId;
            news.UpdatedAt = DateTime.UtcNow;
            await newsRepo.UpdateAsync(news);
            var currentMenuIds = await menuNewsRepo.GetMenuIdsByNewsIdAsync(news.Id, cancellationToken);
            var requestedMenuIds = new List<int>();
            foreach (var item in request.DanhSachMenus)
            {
                if (!currentMenuIds.Contains(item.Id.Value)) throw new ValidationException(new[]{new ValidationFailure(nameof(item.Id),$"Menu có Id = {item.Id} không liên kết với News này.")});
                var menu = await menuRepo.GetByIdAsync(item.Id.Value);
                if (menu == null)throw new ValidationException(new[]{new ValidationFailure(nameof(item.Id),$"Menu có Id = {item.Id} không tồn tại.")});
                var normalizedMenuSlug = item.Slug.Trim().ToLowerInvariant();
                var duplicateMenu = await menuRepo.GetBySlugAsync(normalizedMenuSlug);
                if (duplicateMenu != null && duplicateMenu.Id != item.Id.Value)throw new ValidationException(new[]{new ValidationFailure(nameof(item.Slug),$"Slug Menu '{item.Slug}' đã tồn tại.")});
                mapper.Map(item, menu);
                menu.UpdatedAt = DateTime.UtcNow;
                await menuRepo.UpdateAsync(menu);
                requestedMenuIds.Add(item.Id.Value);
            }
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
            if (!hasCountry &&!hasProvince &&!hasWard &&!hasAddress) {return null;}
            if (!hasCountry || !hasProvince || !hasWard || !hasAddress){throw new ValidationException(new[]{ new ValidationFailure("Address","Phải nhập đầy đủ quốc gia, tỉnh/thành phố, phường/xã và địa chỉ.")});}
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