namespace LMS.Domain.Entities.Orders
{
    public class SellOrder : Order
    {
        // Navigation property:
        public ICollection<OrderItem> OrderItems { get; set; }

        public SellOrder()
        {
            OrderItems = new HashSet<OrderItem>();
        }
    }
}
