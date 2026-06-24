    using Application.Requests.XuLyMenu;
using FluentValidation;

namespace Application.Validators.XuLyMenu;
public class UpdateMenuNewsItemRequestValidator : AbstractValidator<UpdateMenuNewsItemRequest>
{
    public UpdateMenuNewsItemRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .When(x => x.Id.HasValue)
            .WithMessage("Id News phải lớn hơn 0.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Tiêu đề News không được để trống.")
            .MaximumLength(255).WithMessage("Tiêu đề News tối đa 255 ký tự.");

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Slug News không được để trống.")
            .MaximumLength(255).WithMessage("Slug News tối đa 255 ký tự.")
            .Matches("^[a-zA-Z0-9-_]+$")
            .WithMessage("Slug chỉ được chứa chữ, số, gạch ngang hoặc gạch dưới.");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Nội dung News không được để trống.");

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
                var allEmpty  = !x.ProvinceId.HasValue && !x.WardId.HasValue && string.IsNullOrWhiteSpace(x.Address);
                var allFilled = x.ProvinceId.HasValue  && x.WardId.HasValue  && !string.IsNullOrWhiteSpace(x.Address);
                return allEmpty || allFilled;
            })
            .WithMessage("Phải nhập đầy đủ tỉnh/thành phố, phường/xã và địa chỉ hoặc để trống cả 3.");
    }
}
