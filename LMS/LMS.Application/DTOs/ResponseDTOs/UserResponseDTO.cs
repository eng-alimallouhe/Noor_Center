using LMS.Domain.Entities.Users;

namespace LMS.Application.DTOs.ResponseDTOs
{
    public class UserResponseDTO
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastLogIn { get; set; } = DateTime.UtcNow;
        public ICollection<Notification> Notifications { get; set; } = [];
    }
}