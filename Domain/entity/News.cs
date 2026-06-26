namespace Domain.entity
{
    public class News : BaseId
    {
        public string ? Title { get; set; }
        public string ? Slug { get; set; }
        public string ? Content { get; set; }
        public string ? Thumbnail { get; set; }
        public ICollection<MenuNews> MenuNews { get; set; } = new List<MenuNews>();
        public string? Address { get; set; }
        public int? WardId { get; set; }
        public WebsiteLocalizationWard? Ward { get; set; }
    }
}