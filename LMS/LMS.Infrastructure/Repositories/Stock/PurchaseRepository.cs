using LMS.Domain.Entities.Stock;
using LMS.Infrastructure.DbContexts;

namespace LMS.Infrastructure.Repositories.Stock
{
    public class PurchaseRepository : BaseRepository<Purchase>
    {
        private readonly AppDbContext _context;
        public PurchaseRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public override async Task DeleteAsync(Guid id)
        {
            var purchase = await _context.Purchases.FindAsync(id);
            if (purchase is not null)
            {
                purchase.IsActive = false;
                purchase.UpdatedAt = DateTime.UtcNow;
                _context.Purchases.Update(purchase);
                await SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Purchase not found");
            }
        }
    }
}
