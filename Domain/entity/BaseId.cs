namespace Domain.entity
{
    public abstract class BaseId
    {
        public int Id { get; set; }
        public bool is_deleted { get; set; } = false;
        public DateTime ? created_at { get; set; }
        public DateTime ? updated_at { get; set; }
        public DateTime ? deleted_at { get; set; }
    }
}