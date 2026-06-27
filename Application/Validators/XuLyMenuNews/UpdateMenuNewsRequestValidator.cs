using Application.Requests.XuLyMenuNews;
using FluentValidation;

namespace Application.Validators.XuLyMenuNews;

public class UpdateMenuNewsRequestValidator
    : AbstractValidator<UpdateMenuNewsRequest>
{
    public UpdateMenuNewsRequestValidator()
    {
        RuleFor(x => x.OldMenuId).GreaterThan(0).WithMessage("OldMenuId phải lớn hơn 0.");

        RuleFor(x => x.OldNewsId).GreaterThan(0).WithMessage("OldNewsId phải lớn hơn 0.");

        RuleFor(x => x.MenuId).GreaterThan(0).WithMessage("MenuId phải lớn hơn 0.");

        RuleFor(x => x.NewsId).GreaterThan(0).WithMessage("NewsId phải lớn hơn 0.");
    }
}
