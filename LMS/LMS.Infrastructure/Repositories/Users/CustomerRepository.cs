using LMS.Domain.Entities.Users;
using LMS.Infrastructure.DbContexts;

namespace LMS.Infrastructure.Repositories.Users
{
    public class CustomerRepository : BaseRepository<Customer>
    {
        private readonly AppDbContext _context;

        public CustomerRepository(
            AppDbContext context)
            : base(context)
        {
            _context = context;
        }

        public override async Task DeleteAsync(Guid id)
        {
            var user = await _context.Customers.FindAsync(id);

            if (user == null)
            {
                throw new Exception("Not found");
            }

            user.IsDeleted = true;
            _context.Customers.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
