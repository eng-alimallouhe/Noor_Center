using LMS.Domain.Entities.Stock;
using LMS.Infrastructure.DbContexts;

namespace LMS.Infrastructure.Repositories.Stock
{
    public class DiscountRepository : BaseRepository<Discount>
    {
        private readonly AppDbContext _context;

        public DiscountRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task DeleteAsync(Guid id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author is not null)
            {
                author.IsActive = false;
                author.UpdatedAt = DateTime.Now;
                _context.Authors.Update(author);
                await SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Author not found");
            }
        }
    }
}
