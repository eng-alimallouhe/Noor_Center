using LMS.Domain.Entities.Stock;

namespace LMS.Domain.Entities.Orders
{
    public class CartItem
    {
        // Primary key:
        public Guid CartItemId { get; set; }

        //Foreign Key: CartId ==> one(Cart)-to-many(CartItem) relationship
        public Guid CartId { get; set; }

        //Foreign Key: ProductId ==> one(Product)-to-many(CartItem) relationship
        public Guid ProductId { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }

        //Timestamp:
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation property:
        public Cart Cart { get; set; }
        public Product Product { get; set; }

        public CartItem()
        {
            CartItemId = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            Cart = null!;
            Product = null!;
        }
    }
}
