namespace MyNet6Demo.Domain.Abstracts
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; } = "System";

        public DateTime LastUpdatedAt { get; set; }

        public string LastUpdateBy { get; set; } = "System";
    }
}