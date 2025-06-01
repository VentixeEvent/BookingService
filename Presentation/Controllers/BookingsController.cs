using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookingsController(IBookingService bookingsService) : ControllerBase
{
    private readonly IBookingService _bookingsService = bookingsService;

    [HttpPost]
    public async Task<IActionResult> Create (CreateBookingRequest request) 
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _bookingsService.CreateBookingAsync(request);
        return result.Success
            ? Ok()
            : StatusCode(StatusCodes.Status500InternalServerError, "Unable to create booking.");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, UpdateBookingRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _bookingsService.UpdateBookingAsync(id, request);
        return result.Success
            ? Ok()
            : result.Error == "Booking not found"
                ? NotFound(result.Error)
                : StatusCode(StatusCodes.Status500InternalServerError, result.Error ?? "Unable to update booking.");
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _bookingsService.GetBookingsAsync();
        return result.Success
            ? Ok(result.Result)
            : StatusCode(StatusCodes.Status500InternalServerError, result.Error ?? "Unable to retrieve bookings.");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var result = await _bookingsService.GetBookingAsync(id);
        return result.Success
            ? Ok(result.Result)
            : result.Error == "Booking Not Found"
                ? NotFound(result.Error)
                : StatusCode(StatusCodes.Status500InternalServerError, result.Error ?? "Unable to retrieve booking.");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remove(string id)
    {
        var result = await _bookingsService.RemoveBookingAsync(id);
        return result.Success
            ? NoContent()
            : result.Error == "Booking not found"
                ? NotFound(result.Error)
                : StatusCode(StatusCodes.Status500InternalServerError, result.Error ?? "Unable to remove booking.");
    }
}

