using System.ComponentModel.DataAnnotations;

namespace IMS_WebApplication.Models
{
    public class Login
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [MaxLength(50)]
        public required string UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        [MaxLength(50)]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [MaxLength(50)]
        public required string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [MaxLength(50)]
        public required string ConfirmPassword { get; set; }
        public byte[] PasswordHash { get; internal set; }
        public byte[] PasswordSalt { get; internal set; }
    }
}
