using LMS.Domain.Entities.Users;

namespace LMS.Application.DTOs.ResponseDTOs
{
    public class AuthenticationResponseDTO
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public UserResponseDTO User { get; set; } = null!;
    }
}