using Application.Requests.XuLyMenuNews;
using FluentValidation;

namespace Application.Validators.XuLyMenuNews;

public class GetMenuNewsByIdRequestValidator
    : AbstractValidator<GetMenuNewsByIdRequest>
{
    public GetMenuNewsByIdRequestValidator()
    {
        RuleFor(x => x.MenuId).GreaterThan(0).WithMessage("MenuId không hợp lệ.");

        RuleFor(x => x.NewsId).GreaterThan(0).WithMessage("NewsId không hợp lệ.");
    }
}
