using Domain.entity;
using Domain.repositories;
using Application.Requests.XuLyMenu;
using MediatR;
using FluentValidation;
using FluentValidation.Results;

namespace Application.XuLyMenu.UseCases
{
    public class CreateMenuUseCase : IRequestHandler<CreateMenuRequest, bool>
    {
        private readonly IMenuRepo menuRepo;
        private readonly INewsRepo newsRepo;
        private readonly IUnitOfWork uow;
        private readonly IWebsiteLocalizationWardRepo wardRepo;
        private readonly IWebsiteLocalizationRepo localizationRepo;

        public CreateMenuUseCase(IMenuRepo mRepo,INewsRepo nRepo,IUnitOfWork uowR,IWebsiteLocalizationWardRepo wRepo,IWebsiteLocalizationRepo lRepo)
        {
            menuRepo  = mRepo;
            newsRepo  = nRepo;
            uow       = uowR;
            wardRepo  = wRepo;
            localizationRepo=lRepo;
        }
        public async Task<bool> Handle(CreateMenuRequest request, CancellationToken cancellationToken)
        {
            var existing = await menuRepo.GetBySlugAsync(request.Slug.Trim().ToLower());
            if (existing != null)
                {
                    throw new ValidationException(new[]{new ValidationFailure(nameof(request.Slug),$"Slug menu '{request.Slug}' đã tồn tại.")});
                }
            await uow.BeginTransactionAsync(cancellationToken);
            try
            {
                var menu = new Menu
                {
                    Name       = request.Name,
                    Slug       = request.Slug.Trim().ToLower(),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                foreach (var item in request.DanhSachNews)
                {
                    var news = await newsRepo.GetBySlugAsync(item.Slug.Trim().ToLower());
                    if (news != null)
                    {
                        if (!string.Equals(news.Title?.Trim(), item.Title.Trim(), StringComparison.OrdinalIgnoreCase))
                        {
                            throw new ValidationException($"Slug news '{item.Slug}' đã tồn tại với tiêu đề khác.");
                        }
                        menu.MenuNews.Add(new MenuNews
                            {
                                NewsId = news.Id
                            });
                            continue;
                    }
                    var wardId = await ResolveWardIdAsync(item.CountryKey,item.ProvinceId,item.WardId,item.Address,cancellationToken);
                    news = new Domain.entity.News
                    {
                        Title      = item.Title,
                        Slug       = item.Slug.Trim().ToLower(),
                        Content    = item.Content,
                        Thumbnail  = item.Thumbnail,
                        Address    = string.IsNullOrWhiteSpace(item.Address) ? null : item.Address.Trim(),
                        WardId     = wardId,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    await newsRepo.AddAsync(news);
                    menu.MenuNews.Add(new MenuNews
                        {
                            News = news 
                        });
                }
                await menuRepo.AddAsync(menu);
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
                throw new ValidationException(new[]{ new ValidationFailure("Address","Phải nhập đầy đủ quốc gia, tỉnh/thành phố, phường/xã và địa chỉ.")});
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
}