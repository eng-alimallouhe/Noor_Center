using LMS.Domain.Entities.Users;
using LMS.Infrastructure.DbContexts;

namespace LMS.Infrastructure.Repositories.Users
{
    public class DepartmentRepository : BaseRepository<Department>
    {
        private readonly AppDbContext _context;
        public DepartmentRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public override async Task DeleteAsync(Guid id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department != null)
            {
                department.IsActive = false;
                _context.Departments.Update(department);
                await SaveChangesAsync();
            }
            else
            {
                throw new Exception("Department not found");
            }
        }


    }
}
