using LMS.Domain.Entities.Users;
using LMS.Domain.Interfaces;

namespace LMS.Infrastructure.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task LockUser(Guid userId);
    }
}
