using LMS.Domain.Entities.Orders;
namespace LMS.Domain.Entities.Stock
{
    public class Product
    {
        // Primary key:
        public Guid ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;
        public decimal ProductPrice { get; set; }
        public int ProductStock { get; set; }
        public string ImgUrl { get; set; } = string.Empty;

        //soft delete
        public bool IsActive { get; set; }

        //Timestamp:
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation property:
        public ICollection<Category> Categories { get; set; }
        public ICollection<Discount> Discounts { get; set; }
        public ICollection<InventoryLog> Logs { get; set; }
        public ICollection<CartItem> CartItems { get; set; }

        public Product()
        {
            ProductId = Guid.NewGuid();
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            Categories = new List<Category>();
            Discounts = new List<Discount>();
            Logs = new List<InventoryLog>();
            CartItems = new List<CartItem>();
        }
    }
}
