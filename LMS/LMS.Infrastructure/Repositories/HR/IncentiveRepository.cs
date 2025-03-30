using LMS.Domain.Entities.HR;
using LMS.Infrastructure.DbContexts;

namespace LMS.Infrastructure.Repositories.HR
{
    public class IncentiveRepository : BaseRepository<Incentive>
    {
        private readonly AppDbContext _context;
        public IncentiveRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public override async Task DeleteAsync(Guid id)
        {
            var incentive = await _context.Incentives.FindAsync(id);
            if (incentive is not null)
            {
                incentive.IsActive = false;
                _context.Incentives.Update(incentive);
                await SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Incentive not found");
            }
        }
    }

}
