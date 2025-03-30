namespace LMS.Domain.Entities.Stock
{
    public class Publisher
    {
        // Primary key:
        public Guid PublisherId { get; set; }

        public string PublisherName { get; set; } = string.Empty;
        public string PublisherDescription { get; set; } = string.Empty;

        //soft delete
        public bool IsActive { get; set; }

        //Timestamp:
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation property:
        public ICollection<Book> Books { get; set; }

        public Publisher()
        {
            PublisherId = Guid.NewGuid();
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            Books = new List<Book>();
        }
    }

}
