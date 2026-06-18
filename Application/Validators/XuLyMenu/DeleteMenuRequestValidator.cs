using Application.Requests.XuLyMenu;
using FluentValidation;

namespace Application.Validators.XuLyMenu;

public class DeleteMenuRequestValidator : AbstractValidator<DeleteMenuRequest>
{
    public DeleteMenuRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id menu không hợp lệ.");
    }
}