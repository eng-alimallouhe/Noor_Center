using LMS.Domain.Entities.Financial;
using LMS.Infrastructure.DbContexts;

namespace LMS.Infrastructure.Repositories.OrderManagement
{
    public class FinancialRevenueRepository : BaseRepository<FinancialRevenue>
    {
        private readonly AppDbContext _context;
        public FinancialRevenueRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public override async Task DeleteAsync(Guid id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment is not null)
            {
                payment.IsActive = false;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Payment not found");
            }
        }
    }
}
