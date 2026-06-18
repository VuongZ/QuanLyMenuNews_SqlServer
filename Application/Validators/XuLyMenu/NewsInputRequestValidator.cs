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
    }
}