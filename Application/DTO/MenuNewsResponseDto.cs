namespace Application.DTO;

public class MenuNewsResponseDto
{
    public int MenuId { get; set; }
    public int NewsId { get; set; }
    public string MenuName { get; set; } = string.Empty;
    public string NewsTitle { get; set; } = string.Empty;
}
