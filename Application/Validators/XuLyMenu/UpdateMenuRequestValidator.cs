using Application.Requests.XuLyMenu;
using FluentValidation;

namespace Application.Validators.XuLyMenu;

public class UpdateMenuRequestValidator : AbstractValidator<UpdateMenuRequest>
{
    public UpdateMenuRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id menu không hợp lệ.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên menu không được để trống.")
            .MaximumLength(255).WithMessage("Tên menu tối đa 255 ký tự.");

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Slug menu không được để trống.")
            .MaximumLength(255).WithMessage("Slug menu tối đa 255 ký tự.")
            .Matches("^[a-zA-Z0-9-_]+$")
            .WithMessage("Slug menu chỉ được chứa chữ, số, dấu gạch ngang hoặc gạch dưới.");
        RuleFor(x => x.DanhSachNews)
                .NotNull()
                .WithMessage("Danh sách News không được null.");

        RuleForEach(x => x.DanhSachNews)
                .SetValidator(new UpdateMenuNewsItemRequestValidator());

        RuleFor(x => x.DanhSachNews)
                .Must(items =>
                {
                    var slugs = items
                        .Select(x => x.Slug.Trim().ToLowerInvariant())
                        .ToList();

                    return slugs.Count == slugs.Distinct().Count();
                })
                .When(x => x.DanhSachNews != null)
                .WithMessage("Danh sách News không được trùng slug.");      
                }
}