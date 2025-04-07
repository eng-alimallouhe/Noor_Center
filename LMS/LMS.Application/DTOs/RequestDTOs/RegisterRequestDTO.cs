using System.ComponentModel.DataAnnotations;

namespace LMS.Application.DTOs.RequestDTOs
{
    public class RegisterRequestDTO
    {
        [Required(ErrorMessage = "Name Is Required")]
        [MaxLength(60, ErrorMessage = "Max lenght is 60 character")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "User Is Required")]
        [MaxLength(60, ErrorMessage = "Max lenght is 60 character")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email Is Required")]
        [EmailAddress(ErrorMessage = "you must provide a valid number")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password Is Required")]
        [MaxLength(60, ErrorMessage = "Max lenght is 60 character")]
        public string Password { get; set; } = string.Empty;
    }
}
