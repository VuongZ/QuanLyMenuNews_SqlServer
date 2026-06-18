namespace Domain.entity
{
    public class Menu : BaseId
    {
        public string ? Name { get; set; }
        public string ? Slug { get; set; }

        public ICollection<News> News { get; set; } = new List<News>();


    }
}