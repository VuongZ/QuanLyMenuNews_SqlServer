using Application.DTO;
using Domain.entity;

namespace Application.Usecase.XuLyMenuNews
{
    internal static class MenuNewsMapper
    {
        public static MenuNewsResponseDto ToDto(MenuNews menuNews)
        {
            return new MenuNewsResponseDto
            {
                MenuId = menuNews.MenuId,
                NewsId = menuNews.NewsId,
                MenuName = menuNews.Menu?.Name ?? string.Empty,
                NewsTitle = menuNews.News?.Title ?? string.Empty
            };
        }
    }
}
