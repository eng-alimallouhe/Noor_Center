namespace LMS.Domain.Entities.Stock
{
    public class Category
    {
        public Guid CategoryId { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public string CategoryDescription { get; set; } = string.Empty;

        //soft delete: 
        public bool IsActive { get; set; }

        //timestamp:
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<Product> Products { get; set; }

        public Category()
        {
            CategoryId = Guid.NewGuid();
            IsActive = true; 
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            Products = new List<Product>();
        }
    }
}
