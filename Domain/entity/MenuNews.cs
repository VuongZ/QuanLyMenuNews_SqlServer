namespace Domain.entity
{
    public class MenuNews
    {
        public int MenuId { get; set; }
        public int NewsId { get; set; }

        public Menu? Menu { get; set; }
        public News? News { get; set; }
    }
}
