namespace LMS.Domain.Entities.Stock
{
    public class Discount
    {
        // Primary key:
        public Guid DiscountId { get; set; }

        // Foreign key:
        public Guid ProductId { get; set; }

        public decimal DiscountPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; } = true;

        //Timestamp:
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property:
        public Product Product { get; set; } = null!;
    }

}
