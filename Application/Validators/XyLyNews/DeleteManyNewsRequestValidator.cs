using Application.Requests.XuLyNews;
using FluentValidation;

namespace Application.Validators.XuLyNews;

public class DeleteManyNewsRequestValidator: AbstractValidator<DeleteManyNewsRequest>
{
    public DeleteManyNewsRequestValidator()
    {
        RuleFor(x => x.Ids)
            .NotNull()
            .NotEmpty()
            .WithMessage("Danh sách Id News không được để trống.");

        RuleForEach(x => x.Ids)
            .GreaterThan(0)
            .WithMessage("Id News phải lớn hơn 0.");

        RuleFor(x => x.Ids)
            .Must(ids =>
                ids.Distinct().Count() == ids.Count)
            .WithMessage(
                "Danh sách Id News không được trùng nhau.")
            .When(x => x.Ids != null);
    }
}