using LMS.Domain.Entities.Stock;
using LMS.Infrastructure.DbContexts;

namespace LMS.Infrastructure.Repositories.Stock
{
    public class GenreRepository : BaseRepository<Genre>
    {
        private readonly AppDbContext _context;
        public GenreRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public override async Task DeleteAsync(Guid id)
        {
            var genre = await _context.Genres.FindAsync(id);
            if (genre is not null)
            {
                genre.IsActive = false;
                genre.UpdatedAt = DateTime.UtcNow;
                _context.Genres.Update(genre);
                await SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Genre not found");
            }
        }
    }
}
