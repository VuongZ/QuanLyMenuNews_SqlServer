using Application.Requests.XuLyMenu;
using AutoMapper;
using Domain.repositories;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Application.Usecase.XuLyMenu;

public class UpdateMenuUseCase : IRequestHandler<UpdateMenuRequest, bool>
{
        private readonly IMenuRepo menuRepo;
        private readonly INewsRepo newsRepo;
        private readonly IUnitOfWork uow;
        private readonly IWebsiteLocalizationWardRepo wardRepo;
        private readonly IWebsiteLocalizationRepo localizationRepo;
        private readonly IMapper mapper;
        public UpdateMenuUseCase(IMapper imapper,IMenuRepo mRepo,INewsRepo nRepo,IUnitOfWork uowR,IWebsiteLocalizationWardRepo wRepo,IWebsiteLocalizationRepo lRepo)
        {
            menuRepo  = mRepo;
            newsRepo  = nRepo;
            uow       = uowR;
            wardRepo  = wRepo;
            localizationRepo=lRepo;
            mapper=imapper;
        }
        public async Task<bool> Handle(UpdateMenuRequest request,CancellationToken cancellationToken)
        {
            var menuProjection =  menuRepo.GetByIdWithNewsAsync(request.Id)
            .Select(m => new
            {
                m.Id,
                m.Slug,
                NewsIds = m.MenuNews.Select(mn => (int)mn.NewsId).ToArray()
            }).FirstOrDefault();
            if (menuProjection == null)throw new ValidationException(new[] { new ValidationFailure(nameof(request.Id), $"Menu có Id = {request.Id} không tồn tại.")});
            var menu = await menuRepo.GetByIdAsync(request.Id);
            var duplicateMenu =await menuRepo.GetBySlugAsync(request.Slug.Trim().ToLowerInvariant());
            if (duplicateMenu != null && duplicateMenu.Id != request.Id)
            {
                throw new ValidationException(new[]{new ValidationFailure(nameof(request.Slug),$"Slug Menu '{request.Slug}' đã tồn tại.")});
            }
            await uow.BeginTransactionAsync(cancellationToken);
            try
            {
            mapper.Map(request, menu);
            menu.UpdatedAt = DateTime.UtcNow;
            foreach (var item in request.DanhSachNews)
            {
                var news = await newsRepo.GetByIdAsync(item.Id);
                if (news == null )throw new ValidationException(new[]{new ValidationFailure(nameof(item.Id), "Id của News không tồn tại.")});
                if (!menuProjection.NewsIds.Contains(item.Id))throw new ValidationException(new[] { new ValidationFailure(nameof(item.Id),$"News có Id = {item.Id} không liên kết với Menu này.")});
                var duplicateNews = await newsRepo.GetBySlugAsync(item.Slug.Trim().ToLowerInvariant());
                if (duplicateNews != null && duplicateNews.Id != item.Id)throw new ValidationException(new[]{ new ValidationFailure(nameof(item.Slug), $"Slug News '{item.Slug}' đã tồn tại.")});
                var resolvedWardId = await ResolveWardIdAsync(item.CountryKey,item.ProvinceId, item.WardId, item.Address, cancellationToken);
                mapper.Map(item, news);
                news.WardId    = resolvedWardId;
                news.UpdatedAt = DateTime.UtcNow;
                await newsRepo.UpdateAsync(news);
                }
                    await menuRepo.UpdateAsync(menu);
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