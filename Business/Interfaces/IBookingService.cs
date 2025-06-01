using Business.Models;

namespace Business.Interfaces
{
    public interface IBookingService
    {
        Task<BookingResult> CreateBookingAsync(CreateBookingRequest request);
        Task<BookingResult> UpdateBookingAsync(string bookingId, UpdateBookingRequest request);
        Task<BookingResult> RemoveBookingAsync(string bookingId);
        Task<BookingResult<IEnumerable<Booking>>> GetBookingsAsync();
        Task<BookingResult<Booking?>> GetBookingAsync(string bookingId);
    }
}