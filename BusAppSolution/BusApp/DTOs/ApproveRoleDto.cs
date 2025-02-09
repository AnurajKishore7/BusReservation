using System.ComponentModel.DataAnnotations;

namespace BusApp.DTOs
{
    public class ApproveRoleDto
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public bool IsApproved { get; set; }
    }
}
