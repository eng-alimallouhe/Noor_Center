using System.ComponentModel.DataAnnotations;

namespace LMS.Application.DTOs.RequestDTOs
{
    public class AuthorizationRequestDTO
    {
        [Required(ErrorMessage ="Access token is required")]
        public string AccessToken { get; set; } = string.Empty;

        [Required(ErrorMessage = "Referesh token is required")]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
