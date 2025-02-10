using System.ComponentModel.DataAnnotations;

namespace BusApp.DTOs.TripManage
{
    public class TripAddDto
    {
        [Required(ErrorMessage = "BusRouteId is required.")]
        public int BusRouteId { get; set; }

        [Required(ErrorMessage = "BusId is required.")]
        public int BusId { get; set; }

        [Required(ErrorMessage = "Departure Time is required.")]
        public DateTime DepartureTime { get; set; }

        [Required(ErrorMessage = "Arrival Time is required.")]
        public DateTime ArrivalTime { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        public decimal Price { get; set; }

        //Validation
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ArrivalTime <= DepartureTime)
            {
                yield return new ValidationResult(
                    "Arrival time must be after departure time.",
                    new[] { nameof(ArrivalTime) }
                );
            }
        }
    }
}
