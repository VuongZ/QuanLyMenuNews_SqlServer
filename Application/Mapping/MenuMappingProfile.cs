using Application.Requests.XuLyMenu;
using Application.Requests.XuLyNews;
using AutoMapper;
using Domain.entity;

namespace Application.Mapping;

public class MenuMappingProfile : Profile
{
    public MenuMappingProfile()
    {
        CreateMap<UpdateMenuRequest, Menu>()
            .ForMember(x => x.Id,        opt => opt.Ignore())
            .ForMember(x => x.CreatedAt, opt => opt.Ignore())
            .ForMember(x => x.UpdatedAt, opt => opt.Ignore())
            .ForMember(x => x.IsDeleted, opt => opt.Ignore())
            .ForMember(x => x.MenuNews,  opt => opt.Ignore())
            .ForMember(x => x.Slug,      opt => opt.MapFrom(src => src.Slug.Trim().ToLowerInvariant()));

        CreateMap<UpdateNewsMenuItemRequest, Menu>()
            .ForMember(x => x.Id,        opt => opt.Ignore())
            .ForMember(x => x.CreatedAt, opt => opt.Ignore())
            .ForMember(x => x.UpdatedAt, opt => opt.Ignore())
            .ForMember(x => x.IsDeleted, opt => opt.Ignore())
            .ForMember(x => x.MenuNews,  opt => opt.Ignore())
            .ForMember(x => x.Slug,      opt => opt.MapFrom(src => src.Slug.Trim().ToLowerInvariant()));

        CreateMap<UpdateMenuNewsItemRequest, Domain.entity.News>()
            .ForMember(x => x.Id,        opt => opt.Ignore())
            .ForMember(x => x.CreatedAt, opt => opt.Ignore())
            .ForMember(x => x.UpdatedAt, opt => opt.Ignore())
            .ForMember(x => x.IsDeleted, opt => opt.Ignore())
            .ForMember(x => x.WardId,    opt => opt.Ignore())
            .ForMember(x => x.MenuNews,  opt => opt.Ignore())
            .ForMember(x => x.Slug,      opt => opt.MapFrom(src => src.Slug.Trim().ToLowerInvariant()));

        CreateMap<UpdateNewsRequest, Domain.entity.News>()
            .ForMember(x => x.Id,        opt => opt.Ignore())
            .ForMember(x => x.CreatedAt, opt => opt.Ignore())
            .ForMember(x => x.UpdatedAt, opt => opt.Ignore())
            .ForMember(x => x.IsDeleted, opt => opt.Ignore())
            .ForMember(x => x.WardId,    opt => opt.Ignore())
            .ForMember(x => x.MenuNews,  opt => opt.Ignore())
            .ForMember(x => x.Slug,      opt => opt.MapFrom(src => src.slug.Trim().ToLowerInvariant()))
            .ForMember(x => x.Title,     opt => opt.MapFrom(src => src.title))
            .ForMember(x => x.Content,   opt => opt.MapFrom(src => src.content));

}
}