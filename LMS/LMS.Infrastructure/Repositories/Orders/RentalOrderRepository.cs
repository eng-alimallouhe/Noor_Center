using LMS.Domain.Entities.Orders;
using LMS.Infrastructure.DbContexts;

namespace LMS.Infrastructure.Repositories.Orders
{
    public class RentalOrderRepository : BaseRepository<RentalOrder>
    {
        private readonly AppDbContext _context;
        public RentalOrderRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public override async Task DeleteAsync(Guid id)
        {
            var rentalOrder = await _context.RentalOrders.FindAsync(id);
            if (rentalOrder is not null)
            {
                rentalOrder.IsActive = false;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("RentalOrder not found");
            }
        }
    }
}
