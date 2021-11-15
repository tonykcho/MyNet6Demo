namespace MyNet6Demo.Domain.Abstracts
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        public Guid Guid { get; set; } = Guid.NewGuid();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string CreatedBy { get; set; } = "System";

        public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;

        public string LastUpdateBy { get; set; } = "System";
    }
}