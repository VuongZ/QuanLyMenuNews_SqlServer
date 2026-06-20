namespace Application.DTO;
public class NewsResponseDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public string? Content { get; set; }

    public string? Thumbnail { get; set; }

    public string? Address { get; set; }

    public int? WardId { get; set; }

    public string? FullAddress { get; set; }

    public WardInfoResponseDto? WardInfo { get; set; }

    public IEnumerable<MenuBasicResponseDto> Menus { get; set; }  = Enumerable.Empty<MenuBasicResponseDto>();
}