namespace Domain.entity;

public class WebsiteLocalization
{
    public string KeyLocalization { get; set; } = null!;

    public string Localization { get; set; } = null!;

    public bool IsActived { get; set; }

    public ICollection<WebsiteLocalizationWard> Wards { get; set; }
        = new List<WebsiteLocalizationWard>();
}