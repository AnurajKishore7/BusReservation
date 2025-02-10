using System.ComponentModel.DataAnnotations;

namespace BusApp.DTOs.BusManagement
{
    public class BusDto
    {
        [Required]
        public string BusNo { get; set; } = string.Empty;
        [Required]
        public int OperatorId { get; set; }
        [Required]
        public string BusType { get; set; } = string.Empty; 
        [Required]
        public int TotalSeats { get; set; }
    }

}
