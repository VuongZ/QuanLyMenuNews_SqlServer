using Application.DTO;
using Domain.entity;

namespace Application.Mappers;

public static class NewsMapper
{

    public static NewsResponseDto ToDto(this News n)
    {
        return new NewsResponseDto
        {
            Id        = n.Id,
            Title     = n.Title     ?? string.Empty,
            Slug      = n.Slug      ?? string.Empty,
            Content   = n.Content,
            Thumbnail = n.thumbnail,
            Address   = n.Address,
            WardId    = n.WardId,
            FullAddress = n.Ward == null
                ? n.Address
                : n.Address + ", " + n.Ward.FullName,
            WardInfo = n.Ward == null ? null : n.Ward.ToWardInfoDto()
        };
    }

    public static WardInfoResponseDto ToWardInfoDto(this WebsiteLocalizationWard ward)
    {
        return new WardInfoResponseDto
        {
            WardId     = ward.WardId,
            WardPid    = ward.WardPid,
            Name       = ward.Name       ?? string.Empty,
            NameEn     = ward.NameEn,
            FullName   = ward.FullName   ?? string.Empty,
            FullNameEn = ward.FullNameEn,
            Country    = ward.Localization?.Localization ?? string.Empty,
            WardParent = ward.ToWardParentDto()
        };
    }

    public static WardParentResponseDto ToWardParentDto(this WebsiteLocalizationWard ward)
    {
        return new WardParentResponseDto
        {
            WardId  = ward.WardPid,
            Name    = ward.FullName != null && ward.FullName.Contains(",")
                ? ward.FullName.Substring(ward.FullName.IndexOf(",") + 1).Trim()
                : string.Empty,
            NameEn  = ward.FullNameEn != null && ward.FullNameEn.Contains(",")
                ? ward.FullNameEn.Substring(ward.FullNameEn.IndexOf(",") + 1).Trim()
                : null,
            Country = ward.Localization?.Localization ?? string.Empty
        };
    }

    public static MenuResponseDto ToDto(this Menu m)
    {
        return new MenuResponseDto
        {
            Id   = m.Id,
            Name = m.Name ?? string.Empty,
            Slug = m.Slug ?? string.Empty,
            News = m.News
                .Where(n => !n.is_deleted)
                .Select(n => n.ToDto())
        };
    }
}