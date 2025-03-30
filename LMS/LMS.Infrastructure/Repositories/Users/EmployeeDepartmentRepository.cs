using LMS.Domain.Entities.Users;
using LMS.Infrastructure.DbContexts;

namespace LMS.Infrastructure.Repositories.Users
{
    public class EmployeeDepartmentRepository : BaseRepository<EmployeeDepartment>
    {
        private readonly AppDbContext _context;
        public EmployeeDepartmentRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public override async Task DeleteAsync(Guid id)
        {
            var employeeDepartment = await _context.EmployeeDepartments.FindAsync(id);
            if (employeeDepartment != null)
            {
                employeeDepartment.IsActive = false;
                _context.EmployeeDepartments.Update(employeeDepartment);
                await SaveChangesAsync();
            }

            else
            {
                throw new Exception("EmployeeDepartment not found");
            }
        }


    }
}
