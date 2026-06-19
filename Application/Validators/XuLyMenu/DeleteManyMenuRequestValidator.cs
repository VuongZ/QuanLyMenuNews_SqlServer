using Application.Requests.XuLyMenu;
using FluentValidation;

namespace Application.Validators.XuLyMenu;

public class DeleteManyMenuRequestValidator
    : AbstractValidator<DeleteManyMenuRequest>
{
    public DeleteManyMenuRequestValidator()
    {
        RuleFor(x => x.Ids)
            .NotNull()
            .NotEmpty()
            .WithMessage("Danh sách Id không được để trống.");

        RuleForEach(x => x.Ids)
            .GreaterThan(0)
            .WithMessage("Id Menu phải lớn hơn 0.");

        RuleFor(x => x.Ids)
            .Must(ids => ids.Distinct().Count() == ids.Count)
            .WithMessage("Danh sách Id không được trùng nhau.")
            .When(x => x.Ids != null);
    }
}