using LuxStayApi.Data;
using LuxStayApi.Models;
using LuxStayApi.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LuxStayApi.Controllers.Api;

[Route("api/[controller]")]
public class BookingTourController : Controller 
{
    private readonly ApplicationDbContext _dbContext;

    public BookingTourController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("{id}")]
    // [Authorize(Roles = "ROLE_ADMIN")]
    public async Task<IActionResult> Get(int id)
    {
        var bookingTour = await _dbContext.BookingTours.FindAsync(id);
        if (bookingTour == null) return NotFound(new { message = "Booking Tour not found" });
        return Ok(bookingTour);
    }

    [HttpGet("list-bookingtour")]
    [Authorize(Roles = "ROLE_ADMIN")]
    public async Task<IActionResult> All()
    {
        var bookingTours = await _dbContext.BookingTours.ToListAsync();
        return Ok(bookingTours);
    }


    // [HttpPost("delete/{id}")]
    // // [Authorize(Roles = "ROLE_ADMIN")]
    // public async Task<IActionResult> Delete(int id)
    // {
    //     var user = await _dbContext.Users.FindAsync(id);
    //     if (user == null)
    //     {
    //         return NotFound(new { message = "User not found" });
    //     }

    //     _dbContext.Users.Remove(user);
    //     await _dbContext.SaveChangesAsync();
    //     return Ok(new { 
    //         message = "Xóa thành công UwU", 
    //         user = user, 
    //     });
    // }


    [HttpPost("create")]
    // [Authorize(Roles = "ROLE_ADMIN")]
    public async Task<IActionResult> Create([FromBody] BookingTourViewModel bookingTourViewModel)
    {
        if (bookingTourViewModel == null)
        {
            return BadRequest(new { message = "Booking tour data is required." });
        }

        if (bookingTourViewModel.people <= 0)
        {
            return BadRequest(new { message = "People must be greater than 0." });
        }

        var tour = await _dbContext.Tours.FindAsync(bookingTourViewModel.tour_id);

        await _dbContext.BookingTours.AddAsync(new BookingTour {
            user_id = bookingTourViewModel.user_id,
            tour_id = bookingTourViewModel.tour_id,
            date_booking = DateTime.Now,
            price = tour.price,
            people = bookingTourViewModel.people,
            total_price = bookingTourViewModel.people * tour.price,
        });
        await _dbContext.SaveChangesAsync();

        var bookingTour = await _dbContext.BookingTours.OrderByDescending(t => t.booking_tour_id) 
                                   .FirstOrDefaultAsync();
        return Ok(bookingTour);
    }


    // [HttpPost("update/{id}")]
    // // [Authorize(Roles = "ROLE_ADMIN")]
    // public async Task<IActionResult> Update(int id, [FromBody] BookingViewModel bookingViewModel)
    // {
    //     if (bookingViewModel == null)
    //     {
    //         return BadRequest(new { message = "User data is invalid." });
    //     }

    //     var existingUser = await _dbContext.Users.FindAsync(id);
    //     if (existingUser == null)
    //     {
    //         return NotFound(new { message = "User not found" });
    //     }

    //     existingUser.name = userViewModel.name;
    //     existingUser.phone = userViewModel.phone;
    //     existingUser.email = userViewModel.email;
    //     existingUser.address = userViewModel.address ?? "";
    //     existingUser.gender = userViewModel.gender;
    //     existingUser.role = userViewModel.role ?? "ROLE_USER";

    //     await _dbContext.SaveChangesAsync();
    //     return Ok(new { 
    //         message = "Cập nhật thành công UwU", 
    //         user = existingUser, 
    //     });
    // }

}