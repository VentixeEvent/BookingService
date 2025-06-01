using Data.Entities;

namespace Business.Models;

public class Booking
{
    public string Id { get; set; } = string.Empty;
    public string EventId { get; set; } = string.Empty;
    public DateTime BookingDate { get; set; }
    public int TicketQuantity { get; set; }
    public BookingOwner? BookingOwner { get; set; }
}
