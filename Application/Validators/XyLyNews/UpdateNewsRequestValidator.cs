using Application.Requests.XuLyNews;
using FluentValidation;

namespace Application.Validators.XuLyNews;

public class UpdateNewsRequestValidator : AbstractValidator<UpdateNewsRequest>
{
    public UpdateNewsRequestValidator()
    {
        RuleFor(x => x.id).GreaterThan(0).WithMessage("Id news không hợp lệ.");
        RuleFor(x => x.title).NotEmpty().WithMessage("Title không được để trống.").MaximumLength(255).WithMessage("Title tối đa 255 ký tự.");
        RuleFor(x => x.slug).NotEmpty().WithMessage("Slug không được để trống.").MaximumLength(255).WithMessage("Slug tối đa 255 ký tự.").Matches("^[a-zA-Z0-9-_]+$").WithMessage("Slug chỉ được chứa chữ, số, dấu gạch ngang hoặc gạch dưới.");
        RuleFor(x => x.content).NotEmpty().WithMessage("Content không được để trống.");
        RuleFor(x => x.Thumbnail).MaximumLength(255).WithMessage("Thumbnail tối đa 255 ký tự.");
    }
}
