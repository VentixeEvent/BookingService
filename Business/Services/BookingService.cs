using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;

namespace Business.Services;

public class BookingService(IBookingRepository bookingRepository) : IBookingService
{

    private readonly IBookingRepository _bookingRespository = bookingRepository;

    public async Task<BookingResult> CreateBookingAsync(CreateBookingRequest request)
    {

        var bookingEntity = new BookingEntity
        {
            EventId = request.EventId,
            BookingDate = DateTime.Now,
            TicketQuantity = request.TicketQuantity,
            BookingOwner = new BookingOwnerEntity
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Address = new BookingAddressEntity
                {
                    StreetName = request.StreetName,
                    PostalCode = request.PostalCode,
                    City = request.City,
                }
            }
        };
        var result = await _bookingRespository.AddAsync(bookingEntity);
        return result.Success
            ? new BookingResult { Success = true }
            : new BookingResult { Success = false, Error = result.Error };
    }

    public async Task<BookingResult> UpdateBookingAsync(string bookingId, UpdateBookingRequest request)
    {
        var existingBookingResult = await _bookingRespository.GetAsync(x => x.Id == bookingId);
        if (!existingBookingResult.Success || existingBookingResult.Result == null)
        {
            return new BookingResult { Success = false, Error = "Booking not found" };
        }

        var existingBooking = existingBookingResult.Result;

        existingBooking.TicketQuantity = request.TicketQuantity;

        if (existingBooking.BookingOwner != null)
        {
            existingBooking.BookingOwner.FirstName = request.FirstName;
            existingBooking.BookingOwner.LastName = request.LastName;
            existingBooking.BookingOwner.Email = request.Email;

            if (existingBooking.BookingOwner.Address != null)
            {
                existingBooking.BookingOwner.Address.StreetName = request.StreetName;
                existingBooking.BookingOwner.Address.PostalCode = request.PostalCode;
                existingBooking.BookingOwner.Address.City = request.City;
            }
        }

        var result = await _bookingRespository.UpdateAsync(existingBooking);
        return result.Success
            ? new BookingResult { Success = true }
            : new BookingResult { Success = false, Error = result.Error };
    }

    public async Task<BookingResult> RemoveBookingAsync(string bookingId)
    {
        var existingBookingResult = await _bookingRespository.GetAsync(x => x.Id == bookingId);
        if (!existingBookingResult.Success || existingBookingResult.Result == null)
        {
            return new BookingResult { Success = false, Error = "Booking not found" };
        }

        var result = await _bookingRespository.DeleteAsync(existingBookingResult.Result);
        return result.Success
            ? new BookingResult { Success = true }
            : new BookingResult { Success = false, Error = result.Error };
    }

    public async Task<BookingResult<IEnumerable<Booking>>> GetBookingsAsync()
    {
        var result = await _bookingRespository.GetAllAsync();
        if (!result.Success || result.Result == null)
        {
            return new BookingResult<IEnumerable<Booking>>
            {
                Success = false,
                Error = result.Error ?? "Failed to retrieve bookings"
            };
        }

        var bookings = result.Result.Select(x => new Booking
        {
            Id = x.Id,
            EventId = x.EventId,
            BookingDate = x.BookingDate,
            TicketQuantity = x.TicketQuantity,
            BookingOwner = x.BookingOwner != null ? new BookingOwner
            {
                FirstName = x.BookingOwner.FirstName,
                LastName = x.BookingOwner.LastName,
                Email = x.BookingOwner.Email,
                Address = x.BookingOwner.Address != null ? new BookingAddress
                {
                    StreetName = x.BookingOwner.Address.StreetName,
                    PostalCode = x.BookingOwner.Address.PostalCode,
                    City = x.BookingOwner.Address.City
                } : null
            } : null
        });

        return new BookingResult<IEnumerable<Booking>> { Success = true, Result = bookings };
    }


    public async Task<BookingResult<Booking?>> GetBookingAsync(string bookingId)
    {
        var result = await _bookingRespository.GetAsync(x => x.Id == bookingId);
        if (result.Success && result.Result != null)
        {
            var currentBooking = new Booking
            {
                Id = result.Result.Id,
                EventId = result.Result.EventId,
                BookingDate = result.Result.BookingDate,
                TicketQuantity = result.Result.TicketQuantity,
                BookingOwner = result.Result.BookingOwner != null ? new BookingOwner
                {
                    FirstName = result.Result.BookingOwner.FirstName,
                    LastName = result.Result.BookingOwner.LastName,
                    Email = result.Result.BookingOwner.Email,
                    Address = result.Result.BookingOwner.Address != null ? new BookingAddress
                    {
                        StreetName = result.Result.BookingOwner.Address.StreetName,
                        PostalCode = result.Result.BookingOwner.Address.PostalCode,
                        City = result.Result.BookingOwner.Address.City
                    } : null
                } : null
            };

            return new BookingResult<Booking?> { Success = true, Result = currentBooking };
        }

        return new BookingResult<Booking?> { Success = false, Error = "Booking Not Found" };
    }


}

