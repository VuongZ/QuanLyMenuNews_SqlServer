namespace Application.DTO;

public class WardParentResponseDto
{
    public int WardId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? NameEn { get; set; }

    public string FullName { get; set; } = string.Empty;
}