using LMS.Domain.Entities.Users;
using LMS.Infrastructure.DbContexts;

namespace LMS.Infrastructure.Repositories.Users
{
    public class OtpCodeRepository : BaseRepository<OtpCode>
    {
        private readonly AppDbContext _context;

        public OtpCodeRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public override  async Task DeleteAsync(Guid id)
        {
            var code = await _context.OtpCodes.FindAsync(id);
            if (code != null)
            {
                _context.OtpCodes.Remove(code);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Otp code not found");
            }
        }
    }
}
