using LMS.Domain.Entities.Stock;

namespace LMS.Domain.Entities.Orders
{
    public class OrderItem
    {
        // Primary key:
        public Guid OrderItemId { get; set; }

        //Foreign Key: SellOrderId ==> one(SellOrder)-to-many(OrderItem) relationship
        public Guid SellOrderId { get; set; }

        //Foreign Key: ProductId ==> one(Product)-to-many(OrderItem) relationship
        public Guid ProductId { get; set; }

        public int Quantity { get; set; } = 1;
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalPrice { get; set; }

        //Soft Delete:
        public bool IsActive { get; set; }

        //Timestamp:
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation property:
        public SellOrder SellOrder { get; set; }
        public Product Product { get; set; }

        public OrderItem()
        {
            OrderItemId = Guid.NewGuid();
            Discount = 0;
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            SellOrder = null!;
            Product = null!;
        }
    }
}