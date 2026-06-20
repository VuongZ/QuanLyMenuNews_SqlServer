namespace Application.DTO;

public class MenuResponseDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;
    public IEnumerable<NewsResponseDto> News { get; set; } =  Enumerable.Empty<NewsResponseDto>();
}