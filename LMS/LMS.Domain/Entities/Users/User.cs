using System.Reflection.Emit;

namespace LMS.Domain.Entities.Users
{
    public class User
    {
        //primary key
        public Guid UserId { get; set; }

        //Foreign Key: RoleId ==> one(role)-to-many(users) relationship
        public Guid RoleId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int FailedLoginAttempts { get; set; } = 0;

        //locke user account:
        public bool IsLocked { get; set; } = false;

        public bool IsVerified { get; set; } = false;

        //soft delete:
        public bool IsDeleted { get; set; } = false;

        //Timestamp:
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastLogIn { get; set; } = DateTime.UtcNow;
        public DateTime? LockedUntil { get; set; }

        //Navigation Property:
        public Role Role { get; set; } = null!;

        public ICollection<Notification> Notifications { get; set; } = [];
        
        public OtpCode OtpCode { get; set; } = null!;

        public RefreshToken RefreshToken { get; set; } = null!;


        public User()
        {
            UserId = Guid.NewGuid();
        }
    }
}
