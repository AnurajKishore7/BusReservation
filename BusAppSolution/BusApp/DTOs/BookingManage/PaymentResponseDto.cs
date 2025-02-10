namespace BusApp.DTOs.BookingManage
{
    public class PaymentResponseDto
    {
        public int PaymentId { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
        public DateTime PaymentMadeAt { get; set; }
    }

}
