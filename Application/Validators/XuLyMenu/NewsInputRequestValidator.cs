using Application.Requests.XuLyMenu;
using FluentValidation;

namespace Application.Validators.XuLyMenu;

public class NewsInputRequestValidator : AbstractValidator<NewsInputRequest>
{
    public NewsInputRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Tiêu đề News không được để trống.").MaximumLength(255);
        RuleFor(x => x.Slug).NotEmpty().WithMessage("Slug News không được để trống.").MaximumLength(255).Matches("^[a-zA-Z0-9-_]+$").WithMessage("Slug News không hợp lệ.");
        RuleFor(x => x.Content).NotEmpty().WithMessage("Nội dung News không được để trống.");
        RuleFor(x => x.CountryKey).MaximumLength(32).WithMessage("Mã quốc gia tối đa 32 ký tự.");
        RuleFor(x => x.ProvinceId).GreaterThan(0).When(x => x.ProvinceId.HasValue).WithMessage("ProvinceId phải lớn hơn 0.");
        RuleFor(x => x.WardId).GreaterThan(0).When(x => x.WardId.HasValue).WithMessage("WardId phải lớn hơn 0.");
        RuleFor(x => x.Address).MaximumLength(255).WithMessage("Địa chỉ tối đa 255 ký tự.");
        RuleFor(x => x).Must(HaveCompleteAddress).WithMessage("Phải nhập đầy đủ quốc gia, tỉnh/thành phố, phường/xã và địa chỉ.");
    }
    private static bool HaveCompleteAddress(NewsInputRequest request)
    {
        var hasCountry =!string.IsNullOrWhiteSpace(request.CountryKey);
        var hasProvince =request.ProvinceId.HasValue;
        var hasWard =request.WardId.HasValue;
        var hasAddress =!string.IsNullOrWhiteSpace(request.Address);
        var allEmpty =!hasCountry &&!hasProvince &&!hasWard &&!hasAddress;
        var allProvided =hasCountry && hasProvince &&hasWard &&hasAddress;
        return allEmpty || allProvided;
    }
}
