using LMS.Domain.Entities.Financial;
using LMS.Infrastructure.DbContexts;

namespace LMS.Infrastructure.Repositories.Financial
{
    public class LevelRepository : BaseRepository<LoyaltyLevel>
    {
        private readonly AppDbContext _context;
        public LevelRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public override async Task DeleteAsync(Guid id)
        {
            var level = await _context.Levels.FindAsync(id);
            if (level != null)
            {
                level.IsActive = false;
                _context.Levels.Update(level);
                await SaveChangesAsync();
            }
            else
            {
                throw new Exception("Level not found");
            }
        }
    }
}
