using LMS.Domain.Entities.Stock;
using LMS.Infrastructure.DbContexts;

namespace LMS.Infrastructure.Repositories.Stock
{
    public class BookRepository : BaseRepository<Book>
    {
        private readonly AppDbContext _context;

        public BookRepository(
            AppDbContext context
            ) 
            : base(context)
        {
            _context = context;
        }
        public override async Task DeleteAsync(Guid id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book is not null)
            {
                book.IsActive = false;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Book not found");
            }
        }
    }
}
