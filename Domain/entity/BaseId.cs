namespace Domain.entity
{
    public abstract class BaseId
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime ? CreatedAt { get; set; }
        public DateTime ? UpdatedAt { get; set; }
        public DateTime ? DateledAt { get; set; }
    }
}