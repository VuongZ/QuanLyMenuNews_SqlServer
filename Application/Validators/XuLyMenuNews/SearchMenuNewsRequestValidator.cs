using Application.Requests.XuLyMenuNews;
using FluentValidation;

namespace Application.Validators.XuLyMenuNews;
public class SearchMenuNewsRequestValidator
    : AbstractValidator<SearchMenuNewsRequest>
{
    public SearchMenuNewsRequestValidator()
    {
        RuleFor(x => x.MenuId)
            .GreaterThan(0)
            .When(x => x.MenuId.HasValue)
            .WithMessage("MenuId không hợp lệ.");

        RuleFor(x => x.NewsId)
            .GreaterThan(0)
            .When(x => x.NewsId.HasValue)
            .WithMessage("NewsId không hợp lệ.");
    }
}