using System.ComponentModel.DataAnnotations;

namespace BusApp.DTOs.Auth
{
    public class ClientRegisterDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public DateOnly DOB { get; set; }

        public string? Gender { get; set; }

        [Required, RegularExpression(@"\+91?[0-9]{10}", ErrorMessage = "Invalid Indian mobile number.")]
        public string Contact { get; set; } = string.Empty;
        public bool IsDiabled { get; set; }
    }
}
