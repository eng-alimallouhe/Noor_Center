using LMS.Domain.Entities.Orders;
using LMS.Infrastructure.DbContexts;

namespace LMS.Infrastructure.Repositories.Order
{
    public class DeliveryOrderRepository : BaseRepository<DeliveryOrder>
    {
        private readonly AppDbContext _context;
        public DeliveryOrderRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public override async Task DeleteAsync(Guid id)
        {
            var deliveryOrder = await _context.DeliveryOrders.FindAsync(id);
            if (deliveryOrder is not null)
            {
                deliveryOrder.IsActive = false;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("DeliveryOrder not found");
            }
        }
    }
}
