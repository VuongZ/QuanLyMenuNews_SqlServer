namespace Application.DTO;
public class WardInfoResponseDto
{
    public int WardId { get; set; }

    public int WardPid { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? NameEn { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string? FullNameEn { get; set; }

    public string Country { get; set; } = string.Empty;

    public WardParentResponseDto? WardParent { get; set; }
}