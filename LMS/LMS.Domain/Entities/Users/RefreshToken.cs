namespace LMS.Domain.Entities.Users
{
    public class RefreshToken
    {
        //primary key:
        public Guid RefreshTokenId { get; set; }

        //Foriegn key:
        public Guid UserId { get; set; }

        public string Token { get; set; } = string.Empty;

        public DateTime Expiration { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsRevoked { get; set; } = false;

        //Navigation Property:
        public User User { get; set; } = null!;

        public RefreshToken()
        {
            RefreshTokenId = Guid.NewGuid();
        }
    }
}