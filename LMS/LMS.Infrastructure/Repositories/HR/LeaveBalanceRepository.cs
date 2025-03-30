using LMS.Domain.Entities.HR;
using LMS.Infrastructure.DbContexts;

namespace LMS.Infrastructure.Repositories.HR
{
    public class LeaveBalanceRepository : BaseRepository<LeaveBalance>
    {
        private readonly AppDbContext _context;
        public LeaveBalanceRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public override async Task DeleteAsync(Guid id)
        {
            var leaveBalance = await _context.LeaveBalances.FindAsync(id);
            if (leaveBalance is not null)
            {
                _context.LeaveBalances.Remove(leaveBalance);
                await SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("LeaveBalance not found");
            }
        }
    }

}
