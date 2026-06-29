using Application.DTO;
using Application.Requests.XuLyMenu;
using Domain.repositories;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Application.Usecase.XuLyMenu
{
    public class GetMenuByIdUseCase : IRequestHandler<GetMenuByIdRequest, MenuResponseDto?>
    {
        private readonly IMenuRepo  menuRepo;
        
        public GetMenuByIdUseCase(IMenuRepo  Repo)
        {
            menuRepo = Repo;
        }
        
        public Task<MenuResponseDto?> Handle(GetMenuByIdRequest request, CancellationToken cancellationToken)
        {
            var result =  menuRepo.GetByIdWithNewsAsync(request.id)
            .Select(m => new MenuResponseDto  
            {
            Id   = m.Id,
            Name = m.Name,
            Slug = m.Slug,
            News = m.MenuNews
                .Where(mn => !mn.News.IsDeleted)
                .Select(mn => new NewsResponseDto
                {
                    Id          = mn.News.Id,
                    Title       = mn.News.Title,
                    Slug        = mn.News.Slug,
                    Content     = mn.News.Content,
                    Thumbnail   = mn.News.Thumbnail,
                    Address     = mn.News.Address,
                    WardId      = mn.News.WardId,
                    FullAddress = mn.News.Ward == null
                        ? mn.News.Address
                        : mn.News.Address + ", " + mn.News.Ward.FullName,
                    WardInfo = mn.News.Ward == null ? null : new WardInfoResponseDto
                    {
                        WardId     = mn.News.Ward.WardId,
                        WardPid    = mn.News.Ward.WardPid,
                        Name       = mn.News.Ward.Name,
                        NameEn     = mn.News.Ward.NameEn,
                        FullName   = mn.News.Ward.FullName,
                        FullNameEn = mn.News.Ward.FullNameEn,
                        Country    = mn.News.Ward.Localization == null
                            ? null
                            : mn.News.Ward.Localization.Localization,
                        WardParent = new WardParentResponseDto
                        {
                            WardId  = mn.News.Ward.WardPid,
                            Name    = mn.News.Ward.FullName != null && mn.News.Ward.FullName.Contains(",")? mn.News.Ward.FullName.Substring(mn.News.Ward.FullName.IndexOf(",") + 1).Trim(): string.Empty,
                            NameEn  = mn.News.Ward.FullNameEn != null && mn.News.Ward.FullNameEn.Contains(",")? mn.News.Ward.FullNameEn.Substring(mn.News.Ward.FullNameEn.IndexOf(",") + 1).Trim(): null,
                            Country = mn.News.Ward.Localization == null? null: mn.News.Ward.Localization.Localization,
                            }
                        }
                    })
                }).FirstOrDefault(); 
                if (result == null)
                throw new ValidationException(new[]
                {
                    new ValidationFailure(nameof(request.id), $"ID menu '{request.id}' không tồn tại.")
                });
            return Task.FromResult<MenuResponseDto?>(result); 
        }
    }
}