namespace BusApp.DTOs.BookingManage
{
    public class PaymentRequestDto
    {
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; }
    }

}
