using Application.Requests.XuLyMenu;
using FluentValidation;

namespace Application.Validators.XuLyMenu;

public class CreateMenuRequestValidator : AbstractValidator<CreateMenuRequest>
{
    public CreateMenuRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Tên menu không được để trống.").MaximumLength(255).WithMessage("Tên menu tối đa 255 ký tự.");
        RuleFor(x => x.Slug).NotEmpty().WithMessage("Slug menu không được để trống.").MaximumLength(255).WithMessage("Slug menu tối đa 255 ký tự.").Matches("^[a-zA-Z0-9-_]+$").WithMessage("Slug menu chỉ được chứa chữ, số, dấu gạch ngang hoặc gạch dưới.");
        RuleFor(x => x.DanhSachNews).NotNull().WithMessage("Danh sách news không được null.");
        RuleForEach(x => x.DanhSachNews).SetValidator(new NewsInputRequestValidator());
        RuleFor(x => x.DanhSachNews).Must(list =>
            {
                var slugs = list
                    .Select(x => x.Slug.Trim().ToLower())
                    .ToList();
                return slugs.Count == slugs.Distinct().Count();
            })
            .WithMessage("Danh sách News không được trùng Slug.")
            .When(x => x.DanhSachNews != null && x.DanhSachNews.Any());
    }
}
