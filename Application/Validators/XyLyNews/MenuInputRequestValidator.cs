using Application.Requests.XuLyNews;
using FluentValidation;

namespace Application.Validators.XuLyNews;

public class MenuInputRequestValidator : AbstractValidator<MenuInputRequest>
{
    public MenuInputRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Tên menu không được để trống.").MaximumLength(255).WithMessage("Tên menu tối đa 255 ký tự.");
        RuleFor(x => x.Slug).NotEmpty().WithMessage("Slug menu không được để trống.").MaximumLength(255).WithMessage("Slug menu tối đa 255 ký tự.").Matches("^[a-zA-Z0-9-_]+$").WithMessage("Slug menu chỉ được chứa chữ, số, dấu gạch ngang hoặc gạch dưới.");
    }
}
