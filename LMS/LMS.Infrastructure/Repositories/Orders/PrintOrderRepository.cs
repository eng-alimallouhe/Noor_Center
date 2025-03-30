using LMS.Domain.Entities.Orders;
using LMS.Infrastructure.DbContexts;

namespace LMS.Infrastructure.Repositories.Order
{
    public class PrintOrderRepository : BaseRepository<PrintOrder>
    {
        private readonly AppDbContext _context;
        public PrintOrderRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public override async Task DeleteAsync(Guid id)
        {
            var printOrder = await _context.PrintOrders.FindAsync(id);
            if (printOrder is not null)
            {
                printOrder.IsActive = false;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("PrintOrder not found");
            }
        }
    }
}
