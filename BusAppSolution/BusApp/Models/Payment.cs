using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BusApp.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Booking")]
        public int BookingId { get; set; }

        [Required(ErrorMessage = "Total amount is required")]
        [Column(TypeName = "decimal(10,2)")]
        [Range(0.00, 100000, ErrorMessage = "Total amount must be at least 0.00 and less than 1,00,000")]
        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "Payment method is required")]
        [StringLength(20, ErrorMessage = "Payment method cannot exceed 20 characters")]
        public string? PaymentMethod { get; set; }  // CreditCard, DebitCard, UPI, NetBanking, Wallet


        [Required]
        public DateTime PaymentMadeAt { get; set; } = DateTime.Now; // Defaults to current  time

        [Required(ErrorMessage = "Payment status is required")]
        [RegularExpression("^(Paid|Pending|Failed)$", ErrorMessage = "Status must be Paid, Pending, or Failed")]
        public string? Status { get; set; } // Paid, Pending, Failed


        //Navigation Property
        public Booking? Booking { get; set; }
    }
}
