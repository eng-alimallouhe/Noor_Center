using LMS.Domain.Entities.HR;
using LMS.Infrastructure.DbContexts;

namespace LMS.Infrastructure.Repositories.HR
{
    public class AttendanceRepository: BaseRepository<Attendance>
    {
        private readonly AppDbContext _context;

        public AttendanceRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task DeleteAsync(Guid id)
        {
            var attendance = await _context.Attendances.FindAsync(id);

            if (attendance is not null)
            {
                attendance.IsActive = false;
                _context.Attendances.Update(attendance);
                await SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Attendance not found");
            }
        }
    }

}
