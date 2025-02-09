using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BusApp.Models
{
    public class Trip
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Route ID is required.")]
        [ForeignKey("Route")]
        public int BusRouteId { get; set; }

        [Required(ErrorMessage = "Bus ID is required.")]
        [ForeignKey("Bus")]
        public int BusId { get; set; }

        [Required(ErrorMessage = "Departure time is required.")]
        [DataType(DataType.DateTime)]
        public DateTime DepartureTime { get; set; }

        [Required(ErrorMessage = "Arrival time is required.")]
        [DataType(DataType.DateTime)]
        public DateTime ArrivalTime { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.1, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }


        //Navigation Properties
        public BusRoute? BusRoute { get; set; }
        public Bus? Bus { get; set; }
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
