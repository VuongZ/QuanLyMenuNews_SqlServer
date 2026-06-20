    using Application.Requests.XuLyMenu;
using FluentValidation;

namespace Application.Validators.XuLyMenu;

public class UpdateMenuNewsItemRequestValidator
    : AbstractValidator<UpdateMenuNewsItemRequest>
{
    public UpdateMenuNewsItemRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .When(x => x.Id.HasValue)
            .WithMessage("Id News phải lớn hơn 0.");

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Tiêu đề News không được để trống.")
            .MaximumLength(255)
            .WithMessage("Tiêu đề News tối đa 255 ký tự.");

        RuleFor(x => x.Slug)
            .NotEmpty()
            .WithMessage("Slug News không được để trống.")
            .MaximumLength(255)
            .WithMessage("Slug News tối đa 255 ký tự.")
            .Matches("^[a-zA-Z0-9-_]+$")
            .WithMessage(
                "Slug chỉ được chứa chữ, số, gạch ngang hoặc gạch dưới.");

        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage("Nội dung News không được để trống.");

        RuleFor(x => x.Thumbnail)
            .MaximumLength(255)
            .WithMessage("Thumbnail tối đa 255 ký tự.");

        RuleFor(x => x.Address)
            .MaximumLength(255)
            .WithMessage("Địa chỉ tối đa 255 ký tự.");

        RuleFor(x => x.ProvinceName)
            .MaximumLength(96)
            .WithMessage("Tên tỉnh/thành phố tối đa 96 ký tự.");

        RuleFor(x => x.WardName)
            .MaximumLength(96)
            .WithMessage("Tên phường/xã tối đa 96 ký tự.");

        RuleFor(x => x)
            .Must(HaveCompleteAddress)
            .WithMessage(
                "Phải nhập đầy đủ tỉnh/thành phố, phường/xã và địa chỉ.");
    }

    private static bool HaveCompleteAddress(
        UpdateMenuNewsItemRequest item)
    {
        var hasProvince =!string.IsNullOrWhiteSpace(item.ProvinceName);

        var hasWard = !string.IsNullOrWhiteSpace(item.WardName);

        var hasAddress =!string.IsNullOrWhiteSpace(item.Address);

        var allEmpty =!hasProvince &&!hasWard &&! hasAddress;

        var allProvided =hasProvince &&hasWard && hasAddress;

        return allEmpty || allProvided;
    }
}