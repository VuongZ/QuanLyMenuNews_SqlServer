using Application.Requests.XuLyNews;
using FluentValidation;

namespace Application.Validators.XuLyNews;

public class CreateNewsRequestValidator : AbstractValidator<CreateNewsRequest>
{
    public CreateNewsRequestValidator()
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

        RuleFor(x => x.DanhSachMenus)
            .NotNull().WithMessage("Danh sách menu không được null.");

        RuleForEach(x => x.DanhSachMenus)
            .SetValidator(new MenuInputRequestValidator());

        RuleFor(x => x.DanhSachMenus)
            .Must(list =>
            {
                var slugs = list
                    .Select(x => x.Slug.Trim().ToLower())
                    .ToList();

                return slugs.Count == slugs.Distinct().Count();
            })
            .WithMessage("Danh sách Menu không được trùng Slug.")
            .When(x => x.DanhSachMenus != null && x.DanhSachMenus.Any());
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
            .Must(x =>
            {
                var hasProvince =
                    !string.IsNullOrWhiteSpace(x.ProvinceName);

                var hasWard =
                    !string.IsNullOrWhiteSpace(x.WardName);

                return hasProvince == hasWard;
            })
            .WithMessage(
                "Phải nhập đồng thời cả tỉnh/thành phố và phường/xã.");

        RuleFor(x => x)
            .Must(x =>
            {
                var hasAddress =
                    !string.IsNullOrWhiteSpace(x.Address);

                var hasProvince =
                    !string.IsNullOrWhiteSpace(x.ProvinceName);

                var hasWard =
                    !string.IsNullOrWhiteSpace(x.WardName);

                return !hasAddress || (hasProvince && hasWard);
            })
            .WithMessage(
                "Khi nhập địa chỉ đường, phải nhập đủ tỉnh/thành phố và phường/xã.");
            }
}