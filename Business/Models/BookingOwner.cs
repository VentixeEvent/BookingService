using Data.Entities;

namespace Business.Models;

public class BookingOwner
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public BookingAddress? Address { get; set; }

}
