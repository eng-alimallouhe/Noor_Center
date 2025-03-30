namespace LMS.Domain.Entities.Stock
{
    public class Genre
    {
        // Primary key:
        public int GenreId { get; set; }

        public string GenreName { get; set; } = string.Empty;
        public string GenreDescription { get; set; } = string.Empty;

        //soft delete
        public bool IsActive { get; set; } = true;

        //Timestamp:
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property:
        public ICollection<Book> Books { get; set; } = [];


    }
}
