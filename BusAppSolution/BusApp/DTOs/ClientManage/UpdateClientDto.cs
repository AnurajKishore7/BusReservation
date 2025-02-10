using System.ComponentModel.DataAnnotations;

namespace BusApp.DTOs.ClientManage
{
    public class UpdateClientDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public DateOnly DOB { get; set; }

        public string? Gender { get; set; }

        [Required]
        [RegularExpression(@"\+91?[0-9]{10}", ErrorMessage = "Invalid Indian mobile number.")]
        public string? Contact { get; set; }
        public bool IsDiabled { get; set; }
    }
}
