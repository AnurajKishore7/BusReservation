using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BusApp.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Client")]
        public int ClientId { get; set; }

        [ForeignKey("Trip")]
        public int TripId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime BookedAt { get; set; } = DateTime.Now;  // Default to current time

        [Required(ErrorMessage = "Status is required.")]
        [StringLength(20, ErrorMessage = "Status cannot exceed 20 characters.")]
        [RegularExpression("^(Pending|Confirmed|Cancelled)$", ErrorMessage = "Status must be either Pending, Confirmed, or Cancelled.")]
        public string Status { get; set; } = "Pending"; // Default status


        //Navigation Properties
        public Client? Client { get; set; }
        public Trip? Trip { get; set; }

        public ICollection<TicketPassenger> TicketPassengers { get; set; } = new List<TicketPassenger>();

        public Payment? Payment { get; set; }
    }
}
