namespace LMS.Domain.Entities.Users
{
    public class Notification
    {
        //primary key: 
        public Guid NotificationId { get; set; }

        // Foreign Key: UserId ==> one(user) to many(notifications) relationship
        public Guid UserId { get; set; }

        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false; 
        public DateTime? ReadAt { get; set; }
        public string RedirectUrl { get; set; } = string.Empty;

        // Navigation Property:
        public User User { get; set; } = null!;

        public Notification()
        {
            NotificationId = Guid.NewGuid();
        }

    }
}
