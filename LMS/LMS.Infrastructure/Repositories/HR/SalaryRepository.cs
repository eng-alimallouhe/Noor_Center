
using LMS.Domain.Entities.HR;
using LMS.Infrastructure.DbContexts;

namespace LMS.Infrastructure.Repositories.HR
{
    public class SalaryRepository : BaseRepository<Salary>
    {
        private readonly AppDbContext _context;
        public SalaryRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public override async Task DeleteAsync(Guid id)
        {
            var salary = await _context.Salaries.FindAsync(id);
            if (salary is not null)
            {
                salary.IsActive = false;
                _context.Salaries.Update(salary);
                await SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Salary not found");
            }
        }
    }

}
