using Application.Requests.XuLyNews;
using FluentValidation;

namespace Application.Validators.XuLyNews;

public class DeleteNewsRequestValidator : AbstractValidator<DeleteNewsRequest>
{
    public DeleteNewsRequestValidator()
    {
        RuleFor(x => x.id).GreaterThan(0).WithMessage("Id news không hợp lệ.");
    }
}
