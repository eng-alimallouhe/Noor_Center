using LMS.Domain.Entities.Stock;
using LMS.Infrastructure.DbContexts;

namespace LMS.Infrastructure.Repositories.Stock
{
    public class BookRepository : BaseRepository<Book>
    {
        private readonly AppDbContext _context;
        private readonly ProductRepository _productRepository;

        public BookRepository(
            AppDbContext context,
            ProductRepository productRepository
            ) 
            : base(context)
        {
            _context = context;
            _productRepository = productRepository;
        }
        public override async Task DeleteAsync(Guid id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book is not null)
            {
                await _productRepository.DeleteAsync(book.ProductId);
            }
            else
            {
                throw new KeyNotFoundException("Book not found");
            }
        }
    }
}
