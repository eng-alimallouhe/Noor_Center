using LMS.Domain.Entities.Stock;
using LMS.Infrastructure.DbContexts;

namespace LMS.Infrastructure.Repositories.Stock
{
    public class PublisherRepository : BaseRepository<Publisher>
    {
        private readonly AppDbContext _context;
        public PublisherRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public override async Task DeleteAsync(Guid id)
        {
            var publisher = await _context.Publishers.FindAsync(id);
            if (publisher is not null)
            {
                publisher.IsActive = false;
                publisher.UpdatedAt = DateTime.UtcNow;
                _context.Publishers.Update(publisher);
                await SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Publisher not found");
            }
        }
    }
}
