using BusApp.DTOs.BookingManage;
using BusApp.Models;
using BusApp.Repositories.Interfaces.BookingManage;
using BusApp.Services.Interfaces.BookingManage;

namespace BusApp.Services.Implementations.BookingManage
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepo _bookingRepo;
        private readonly IPaymentRepo _paymentRepo;
        private readonly ITicketPassengerRepo _ticketPassengerRepo;

        public BookingService(IBookingRepo bookingRepo, IPaymentRepo paymentRepo, ITicketPassengerRepo ticketPassengerRepo)
        {
            _bookingRepo = bookingRepo;
            _paymentRepo = paymentRepo;
            _ticketPassengerRepo = ticketPassengerRepo;
        }

        public async Task<BookingResponseDto> CreateBookingAsync(BookingRequestDto bookingRequest)
        {
            if (bookingRequest == null)
                throw new ArgumentNullException(nameof(bookingRequest));

            if (bookingRequest.Passengers == null || !bookingRequest.Passengers.Any())
                throw new ArgumentException("At least one passenger must be provided.");

            if (bookingRequest.Payment == null)
                throw new ArgumentException("Payment details must be provided.");

            try
            {
                // Create booking entity
                var newBooking = new Booking
                {
                    ClientId = bookingRequest.ClientId,
                    TripId = bookingRequest.TripId,
                    Status = "Pending",
                    BookedAt = DateTime.Now
                };

                var createdBooking = await _bookingRepo.CreateBookingAsync(newBooking);
                if (createdBooking == null)
                    throw new InvalidOperationException("Failed to create booking.");

                // Create ticket passengers
                var ticketPassengers = bookingRequest.Passengers.Select(p => new TicketPassenger
                {
                    BookingId = createdBooking.Id,
                    Name = p.Name,
                    SeatNo = p.SeatNo,
                    Contact = p.Contact,
                    IsDisabled = p.IsDisabled
                }).ToList();

                await _ticketPassengerRepo.AddTicketPassengersAsync(ticketPassengers);

                // Process Payment
                var newPayment = new Payment
                {
                    BookingId = createdBooking.Id,
                    TotalAmount = bookingRequest.Payment.TotalAmount,
                    PaymentMethod = bookingRequest.Payment.PaymentMethod,
                    Status = "Pending",
                    PaymentMadeAt = DateTime.Now
                };

                var addedPayment = await _paymentRepo.AddPaymentAsync(newPayment);
                if (addedPayment == null)
                    throw new InvalidOperationException("Payment processing failed.");

                // Construct response DTO
                return new BookingResponseDto
                {
                    BookingId = createdBooking.Id,
                    ClientId = createdBooking.ClientId,
                    TripId = createdBooking.TripId,
                    BookedAt = createdBooking.BookedAt,
                    Status = createdBooking.Status,
                    Passengers = ticketPassengers.Select(tp => new TicketPassengerResponseDto
                    {
                        Id = tp.Id,
                        Name = tp.Name,
                        SeatNo = tp.SeatNo,
                        Contact = tp.Contact,
                        IsDisabled = tp.IsDisabled
                    }).ToList(),
                    Payment = new PaymentResponseDto
                    {
                        PaymentId = addedPayment.Id,
                        TotalAmount = addedPayment.TotalAmount,
                        PaymentMethod = addedPayment.PaymentMethod,
                        Status = addedPayment.Status,
                        PaymentMadeAt = addedPayment.PaymentMadeAt
                    }
                };
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentNullException("Invalid input: " + ex.ParamName, ex);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException("Validation error: " + ex.Message, ex);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("Operation failed: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while processing the booking.", ex);
            }
        }

        public async Task<BookingResponseDto?> GetBookingByIdAsync(int bookingId)
        {
            if (bookingId <= 0)
                throw new ArgumentException("Invalid booking ID.");

            try
            {
                var booking = await _bookingRepo.GetBookingByIdAsync(bookingId);
                if (booking == null)
                    return null; // Return null if booking not found

                var passengers = await _ticketPassengerRepo.GetTicketPassengersByBookingIdAsync(bookingId);
                var payment = await _paymentRepo.GetPaymentByBookingIdAsync(bookingId);

                return new BookingResponseDto
                {
                    BookingId = booking.Id,
                    ClientId = booking.ClientId,
                    TripId = booking.TripId,
                    BookedAt = booking.BookedAt,
                    Status = booking.Status,
                    Passengers = passengers.Select(p => new TicketPassengerResponseDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        SeatNo = p.SeatNo,
                        Contact = p.Contact,
                        IsDisabled = p.IsDisabled
                    }).ToList(),
                    Payment = payment != null ? new PaymentResponseDto
                    {
                        PaymentId = payment.Id,
                        TotalAmount = payment.TotalAmount,
                        PaymentMethod = payment.PaymentMethod,
                        Status = payment.Status,
                        PaymentMadeAt = payment.PaymentMadeAt
                    } : null
                };
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException("Validation error: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching the booking details.", ex);
            }
        }

        public async Task<IEnumerable<BookingResponseDto>> GetBookingsByClientIdAsync(int clientId)
        {
            if (clientId <= 0)
                throw new ArgumentException("Invalid client ID.");

            try
            {
                var bookings = await _bookingRepo.GetBookingsByClientIdAsync(clientId);
                if (bookings == null || !bookings.Any())
                    return new List<BookingResponseDto>(); // Return empty list if no bookings found

                var bookingDtos = new List<BookingResponseDto>();

                foreach (var booking in bookings)
                {
                    var passengers = await _ticketPassengerRepo.GetTicketPassengersByBookingIdAsync(booking.Id);
                    var payment = await _paymentRepo.GetPaymentByBookingIdAsync(booking.Id);

                    bookingDtos.Add(new BookingResponseDto
                    {
                        BookingId = booking.Id,
                        ClientId = booking.ClientId,
                        TripId = booking.TripId,
                        BookedAt = booking.BookedAt,
                        Status = booking.Status,
                        Passengers = passengers.Select(p => new TicketPassengerResponseDto
                        {
                            Id = p.Id,
                            Name = p.Name,
                            SeatNo = p.SeatNo,
                            Contact = p.Contact,
                            IsDisabled = p.IsDisabled
                        }).ToList(),
                        Payment = payment != null ? new PaymentResponseDto
                        {
                            PaymentId = payment.Id,
                            TotalAmount = payment.TotalAmount,
                            PaymentMethod = payment.PaymentMethod,
                            Status = payment.Status,
                            PaymentMadeAt = payment.PaymentMadeAt
                        } : null
                    });
                }

                return bookingDtos;
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException("Validation error: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching bookings for the client.", ex);
            }
        }

        public async Task<IEnumerable<BookingResponseDto>> GetAllBookingsAsync()
        {
            try
            {
                var bookings = await _bookingRepo.GetAllBookingsAsync();
                if (bookings == null || !bookings.Any())
                    return new List<BookingResponseDto>();
                var bookingDtos = new List<BookingResponseDto>();

                foreach (var booking in bookings)
                {
                    var passengers = await _ticketPassengerRepo.GetTicketPassengersByBookingIdAsync(booking.Id);
                    var payment = await _paymentRepo.GetPaymentByBookingIdAsync(booking.Id);

                    bookingDtos.Add(new BookingResponseDto
                    {
                        BookingId = booking.Id,
                        ClientId = booking.ClientId,
                        TripId = booking.TripId,
                        BookedAt = booking.BookedAt,
                        Status = booking.Status,
                        Passengers = passengers.Select(p => new TicketPassengerResponseDto
                        {
                            Id = p.Id,
                            Name = p.Name,
                            SeatNo = p.SeatNo,
                            Contact = p.Contact,
                            IsDisabled = p.IsDisabled
                        }).ToList(),
                        Payment = payment != null ? new PaymentResponseDto
                        {
                            PaymentId = payment.Id,
                            TotalAmount = payment.TotalAmount,
                            PaymentMethod = payment.PaymentMethod,
                            Status = payment.Status,
                            PaymentMadeAt = payment.PaymentMadeAt
                        } : null
                    });
                }

                return bookingDtos;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving all bookings.", ex);
            }
        }

        public async Task<IEnumerable<BookingResponseDto>> GetConfirmedBookingsAsync()
        {
            try
            {
                var confirmedBookings = await _bookingRepo.GetConfirmedBookingsAsync();
                if (confirmedBookings == null || !confirmedBookings.Any())
                    return new List<BookingResponseDto>(); // Return empty list if no confirmed bookings found

                var bookingDtos = new List<BookingResponseDto>();

                foreach (var booking in confirmedBookings)
                {
                    var passengers = await _ticketPassengerRepo.GetTicketPassengersByBookingIdAsync(booking.Id);
                    var payment = await _paymentRepo.GetPaymentByBookingIdAsync(booking.Id);

                    bookingDtos.Add(new BookingResponseDto
                    {
                        BookingId = booking.Id,
                        ClientId = booking.ClientId,
                        TripId = booking.TripId,
                        BookedAt = booking.BookedAt,
                        Status = booking.Status,
                        Passengers = passengers.Select(p => new TicketPassengerResponseDto
                        {
                            Id = p.Id,
                            Name = p.Name,
                            SeatNo = p.SeatNo,
                            Contact = p.Contact,
                            IsDisabled = p.IsDisabled
                        }).ToList(),
                        Payment = payment != null ? new PaymentResponseDto
                        {
                            PaymentId = payment.Id,
                            TotalAmount = payment.TotalAmount,
                            PaymentMethod = payment.PaymentMethod,
                            Status = payment.Status,
                            PaymentMadeAt = payment.PaymentMadeAt
                        } : null
                    });
                }

                return bookingDtos;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving confirmed bookings.", ex);
            }
        }

        public async Task<IEnumerable<BookingResponseDto>> GetConfirmedBookingsByClientIdAsync(int clientId)
        {
            try
            {
                if (clientId <= 0)
                    throw new ArgumentException("Invalid client ID.");

                var confirmedBookings = await _bookingRepo.GetConfirmedBookingsByClientIdAsync(clientId);
                if (confirmedBookings == null || !confirmedBookings.Any())
                    return new List<BookingResponseDto>(); // Return empty list if no confirmed bookings found

                var bookingDtos = new List<BookingResponseDto>();

                foreach (var booking in confirmedBookings)
                {
                    var passengers = await _ticketPassengerRepo.GetTicketPassengersByBookingIdAsync(booking.Id);
                    var payment = await _paymentRepo.GetPaymentByBookingIdAsync(booking.Id);

                    bookingDtos.Add(new BookingResponseDto
                    {
                        BookingId = booking.Id,
                        ClientId = booking.ClientId,
                        TripId = booking.TripId,
                        BookedAt = booking.BookedAt,
                        Status = booking.Status,
                        Passengers = passengers.Select(p => new TicketPassengerResponseDto
                        {
                            Id = p.Id,
                            Name = p.Name,
                            SeatNo = p.SeatNo,
                            Contact = p.Contact,
                            IsDisabled = p.IsDisabled
                        }).ToList(),
                        Payment = payment != null ? new PaymentResponseDto
                        {
                            PaymentId = payment.Id,
                            TotalAmount = payment.TotalAmount,
                            PaymentMethod = payment.PaymentMethod,
                            Status = payment.Status,
                            PaymentMadeAt = payment.PaymentMadeAt
                        } : null
                    });
                }

                return bookingDtos;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving confirmed bookings for the client.", ex);
            }
        }

        public async Task<bool> CancelBookingAsync(int bookingId)
        {
            try
            {
                if (bookingId <= 0)
                    throw new ArgumentException("Invalid booking ID.");

                var booking = await _bookingRepo.GetBookingByIdAsync(bookingId);
                if (booking == null)
                    throw new KeyNotFoundException("Booking not found.");

                if (booking.Status == "Cancelled")
                    throw new InvalidOperationException("The booking is already cancelled.");

                var isCancelled = await _bookingRepo.CancelBookingAsync(bookingId);
                if (!isCancelled)
                    throw new Exception("Failed to cancel the booking.");

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while cancelling the booking.", ex);
            }
        }

        public async Task<bool> UpdateBookingStatusAsync(int bookingId, string status)
        {
            try
            {
                if (bookingId <= 0)
                    throw new ArgumentException("Invalid booking ID.");

                if (string.IsNullOrWhiteSpace(status) ||
                    !new[] { "Pending", "Confirmed", "Cancelled" }.Contains(status))
                    throw new ArgumentException("Invalid status. Allowed values: Pending, Confirmed, Cancelled.");

                var booking = await _bookingRepo.GetBookingByIdAsync(bookingId);
                if (booking == null)
                    throw new KeyNotFoundException("Booking not found.");

                if (booking.Status == status)
                    throw new InvalidOperationException($"Booking is already in '{status}' status.");

                var updatedBooking = await _bookingRepo.UpdateBookingStatusAsync(bookingId, status);
                if (updatedBooking == null)
                    throw new Exception("Failed to update booking status.");

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the booking status.", ex);
            }
        }


    }

}
