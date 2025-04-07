using System.ComponentModel.DataAnnotations;

namespace LMS.Application.DTOs.RequestDTOs
{
    public class LogInRequestDTO
    {
        [Required(ErrorMessage = "EMail is required")]
        [EmailAddress(ErrorMessage ="Provide a valid email address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password Is Required")]
        public string Password { get; set; } = string.Empty;
    }
}
