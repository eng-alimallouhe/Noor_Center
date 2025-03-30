using LMS.Domain.Entities.Orders;
using LMS.Infrastructure.DbContexts;

namespace LMS.Infrastructure.Repositories.Orders
{
    public class SellOrderRepository : BaseRepository<SellOrder>
    {
        private readonly AppDbContext _context;
        public SellOrderRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public override async Task DeleteAsync(Guid id)
        {
            var sellOrder = await _context.SellOrders.FindAsync(id);
            if (sellOrder is not null)
            {
                sellOrder.IsActive = false;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("SellOrder not found");
            }
        }
    }
}
