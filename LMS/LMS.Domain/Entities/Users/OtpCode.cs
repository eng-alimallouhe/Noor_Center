using LMS.Domain.Enums.Users;

namespace LMS.Domain.Entities.Users
{
    public class OtpCode
    {
        //primary Key:
        public Guid OtpCodeId { get; set; }
        
        //foreign Key: UserId ==> one(user) to one(OtpCode) relationship
        public Guid UserId { get; set; }
        
        public string HashedValue { get; set; } = string.Empty;
        public bool IsUsed { get; set; } = false;
        public int FailedAttempts { get; set; }
        public CodeType CodeType { get; set; }

        //timestamp:
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime ExpiredAt { get; set; } = DateTime.Now.AddMinutes(10);
        
        //navigation property:
        public User User { get; set; } = null!;

        public OtpCode()
        {
            OtpCodeId = Guid.NewGuid();
        }
    }
}
