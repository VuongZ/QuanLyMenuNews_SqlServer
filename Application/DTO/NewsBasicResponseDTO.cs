namespace Application.DTO;
public class NewsBasicResponseDTO
{
    public int Id { get; set; }
    public string Title { get; set; } 
    public string Slug { get; set; }
    public string? Content { get; set; }
    public string? Thumbnail { get; set; }
    public string? Address { get; set; }
    public int? WardId { get; set; }
    public string? FullAddress { get; set; }
}