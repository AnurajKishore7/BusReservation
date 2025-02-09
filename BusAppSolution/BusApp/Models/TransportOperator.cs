using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BusApp.Models
{
    public class TransportOperator
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;

        [ForeignKey("User")]
        public string? Email { get; set; }
        [Required]
        [RegularExpression(@"\+91?[0-9]{10}", ErrorMessage = "Invalid Indian mobile number.")]
        public string Contact { get; set; } = string.Empty;


        //Navigation Properties
        public User? User { get; set; }
        public ICollection<Bus> Buses { get; set; } = new List<Bus>();
    }
}
