using LMS.Domain.Entities.Users;
using LMS.Domain.Interfaces;
using LMS.Infrastructure.DbContexts;
using LMS.Infrastructure.Interfaces;
using LMS.Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories.Users
{
    public class UserRepositroy : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepositroy(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ICollection<User>> GettAllAsync(ISpecification<User> specification)
        {
            var query = SpecificationQueryBuilder.GetQuery<User>(_context.Users, specification);
            return await query.ToListAsync();
        }
        
        public async Task<User?> GetByCriteriaAsync(ISpecification<User> specification)
        {
            var query = SpecificationQueryBuilder.GetQuery<User>(_context.Users, specification);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> AddAsync(User entity)
        {
            await _context.Users.AddAsync(entity);
            await SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(User entity)
        {
            _context.Users.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                user.IsDeleted = true;
                await _context.SaveChangesAsync();
                return;
            }
            throw new KeyNotFoundException("user not found");
        }

        public async Task DeleteHardlyAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return;
            }
            throw new KeyNotFoundException("user not found");
        }

        public async Task LockUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                user.IsLocked = true;
                await _context.SaveChangesAsync();
                return;
            }
            throw new KeyNotFoundException("user not found");
        }


        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
