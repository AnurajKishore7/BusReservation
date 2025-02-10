using System.ComponentModel.DataAnnotations;

namespace BusApp.DTOs.TransOP
{
    public class TransOpUpdateDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"\+91?[0-9]{10}", ErrorMessage = "Invalid Indian mobile number.")]
        public string Contact { get; set; } = string.Empty;
    }
}
