namespace Domain.entity;

public class WebsiteLocalizationWard
{
    public int WardId { get; set; }

    public int WardPid { get; set; }

    public string Name { get; set; } = null!;

    public string? NameEn { get; set; }

    public string FullName { get; set; } = null!;

    public string? FullNameEn { get; set; }

    public string KeyLocalization { get; set; } = null!;

    public bool IsActived { get; set; }

    public WebsiteLocalization? Localization { get; set; }

    public ICollection<News> News { get; set; }
        = new List<News>();
}