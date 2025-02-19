using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BusApp.Models
{
    public class Client
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;

        [ForeignKey("User")]
        public string? Email { get; set; }

        public DateOnly DOB { get; set; }
        public string? Gender { get; set; }
        [Required]
        [RegularExpression(@"\+91?[0-9]{10}", ErrorMessage = "Invalid Indian mobile number.")]
        public string? Contact { get; set; }
        public bool IsDisabled { get; set; } = false;
        public bool IsDeleted { get; set; } = false;


        //Naviagation Properites
        public User? User { get; set; }
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    }
}
