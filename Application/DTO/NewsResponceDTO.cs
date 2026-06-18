namespace Application.DTO;
public class NewsResponseDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public string Thumbnail { get; set; } = string.Empty;

    public List<MenuResponseDto> Menus { get; set; } = new();
}