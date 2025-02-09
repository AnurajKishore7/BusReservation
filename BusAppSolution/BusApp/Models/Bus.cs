using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BusApp.Models
{
    public class Bus
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Bus number is required.")]
        [StringLength(10, ErrorMessage = "Bus number cannot be longer than 10 characters.")]
        public string? BusNo { get; set; }

        [Required(ErrorMessage = "Operator ID is required.")]
        [ForeignKey("TransportOperator")]
        public int OperatorId { get; set; }

        [Required(ErrorMessage = "Bus type is required.")]
        [StringLength(30, ErrorMessage = "Bus type cannot be longer than 30 characters.")]
        public string? BusType { get; set; }

        [Required(ErrorMessage = "Total seats are required.")]
        [Range(1, 100, ErrorMessage = "Total seats must be between 1 and 100.")]
        public int TotalSeats { get; set; }


        //Navigation Properties
        public TransportOperator? TransportOperator { get; set; }

        public ICollection<Trip> Trips { get; set; } = new List<Trip>();
    }
}
