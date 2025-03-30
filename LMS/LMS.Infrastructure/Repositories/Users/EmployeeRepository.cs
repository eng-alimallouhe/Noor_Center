using LMS.Domain.Entities.Users;
using LMS.Infrastructure.DbContexts;

namespace LMS.Infrastructure.Repositories.Users
{
    public class EmployeeRepository : BaseRepository<Employee>
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context)
            : base(context)
        {
            _context = context;
        }

        public override async Task DeleteAsync(Guid id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                throw new Exception("Not found");
            }

            employee.IsDeleted = false;
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }
    }
}
