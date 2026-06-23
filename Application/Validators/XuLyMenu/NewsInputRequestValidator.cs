using Application.Requests.XuLyMenu;
using FluentValidation;

namespace Application.Validators.XuLyMenu;

public class NewsInputRequestValidator : AbstractValidator<NewsInputRequest>
{
    public NewsInputRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title không được để trống.")
            .MaximumLength(255).WithMessage("Title tối đa 255 ký tự.");

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Slug không được để trống.")
            .MaximumLength(255).WithMessage("Slug tối đa 255 ký tự.")
            .Matches("^[a-zA-Z0-9-_]+$")
            .WithMessage("Slug chỉ được chứa chữ, số, dấu gạch ngang hoặc gạch dưới.");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content không được để trống.");

        RuleFor(x => x.Thumbnail)
            .MaximumLength(255).WithMessage("Thumbnail tối đa 255 ký tự.");

        RuleFor(x => x.Address)
            .MaximumLength(255).WithMessage("Địa chỉ tối đa 255 ký tự.");
        RuleFor(x => x.ProvinceId)
            .GreaterThan(0)
            .When(x => x.ProvinceId.HasValue)
            .WithMessage("ProvinceId phải lớn hơn 0.");

        RuleFor(x => x.WardId)
            .GreaterThan(0)
            .When(x => x.WardId.HasValue)
            .WithMessage("WardId phải lớn hơn 0.");
        RuleFor(x => x)
            .Must(x =>
            {
                var allEmpty  = !x.ProvinceId.HasValue
                             && !x.WardId.HasValue
                             && string.IsNullOrWhiteSpace(x.Address);

                var allFilled = x.ProvinceId.HasValue
                             && x.WardId.HasValue
                             && !string.IsNullOrWhiteSpace(x.Address);

                return allEmpty || allFilled;
            })
            .WithMessage("Phải nhập đầy đủ tỉnh, phường/xã và địa chỉ hoặc để trống cả 3.");
    }
}