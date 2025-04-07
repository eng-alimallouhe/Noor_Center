using System.ComponentModel.DataAnnotations;

namespace LMS.Application.DTOs.RequestDTOs
{
    public class ResetPasswordRequestDTO
    {
        [Required(ErrorMessage ="Email Is required")]
        [EmailAddress(ErrorMessage ="Provide a valid email")]
        public string Email { get; set; } = string.Empty;
    }
}
