using LMS.Domain.Entities.HR;
using LMS.Infrastructure.DbContexts;

namespace LMS.Infrastructure.Repositories.HR
{
    public class LeaveRepository : BaseRepository<Leave>
    {
        private readonly AppDbContext _context;
        public LeaveRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public override async Task DeleteAsync(Guid id)
        {
            var leave = await _context.Leaves.FindAsync(id);
            if (leave is not null)
            {
                leave.IsActive = false;
                _context.Leaves.Update(leave);
                await SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Leave not found");
            }
        }
    }

}
