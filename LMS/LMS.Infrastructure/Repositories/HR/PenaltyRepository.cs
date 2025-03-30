using LMS.Domain.Entities.HR;
using LMS.Infrastructure.DbContexts;

namespace LMS.Infrastructure.Repositories.HR
{
    public class PenaltyRepository : BaseRepository<Penalty>
    {
        private readonly AppDbContext _context;
        public PenaltyRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public override async Task DeleteAsync(Guid id)
        {
            var penalty = await _context.Penalties.FindAsync(id);
            if (penalty is not null)
            {
                penalty.IsActive = false;
                _context.Penalties.Update(penalty);
                await SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Penalty not found");
            }
        }
    }

}
