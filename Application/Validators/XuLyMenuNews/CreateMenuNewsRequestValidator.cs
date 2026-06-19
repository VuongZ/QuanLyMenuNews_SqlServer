using Application.Requests.XuLyMenuNews;
using FluentValidation;

namespace Application.Validators.XuLyMenuNews;

public class CreateMenuNewsRequestValidator
    : AbstractValidator<CreateMenuNewsRequest>
{
    public CreateMenuNewsRequestValidator()
    {
        RuleFor(x => x.MenuId)
            .GreaterThan(0)
            .WithMessage("MenuId phải lớn hơn 0.");

        RuleFor(x => x.NewsId)
            .GreaterThan(0)
            .WithMessage("NewsId phải lớn hơn 0.");
    }
}