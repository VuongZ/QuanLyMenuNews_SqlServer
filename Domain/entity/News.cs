namespace Domain.entity
{
    public class News : BaseId
    {
        public string ? Title { get; set; }
        public string ? Slug { get; set; }
        public string ? Content { get; set; }
        public string ? thumbnail { get; set; }
        public ICollection<Menu> Menu { get; set; } = new List<Menu>();

    }
}