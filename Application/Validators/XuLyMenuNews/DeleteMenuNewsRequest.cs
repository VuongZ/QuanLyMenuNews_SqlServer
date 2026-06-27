using Application.Requests.XuLyMenuNews;
using FluentValidation;

namespace Application.Validators.XuLyMenuNews;
public class DeleteMenuNewsRequestValidator
    : AbstractValidator<DeleteMenuNewsRequest>
{
    public DeleteMenuNewsRequestValidator()
    {
        RuleFor(x => x.MenuId).GreaterThan(0).WithMessage("MenuId không hợp lệ.");

        RuleFor(x => x.NewsId).GreaterThan(0).WithMessage("NewsId không hợp lệ.");
    }
}
