using System.ComponentModel.DataAnnotations;

namespace BusApp.Models
{
    public class User
    {
        [Key]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public Byte[] PasswordHash { get; set; } = Array.Empty<byte>();
        [Required]
        public Byte[] PasswordSalt { get; set; } = Array.Empty<byte>();

        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; } = "Client";
        public bool IsApproved { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsDeleted { get; set; } = false;

        //Navigation Properties
        public TransportOperator? TransportOperator { get; set; }
        public Client? Client { get; set; }
    }
}
