using LuxStayApi.Data;
using LuxStayApi.Models;
using LuxStayApi.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LuxStayApi.Controllers.Api;

[Route("api/[controller]")]
public class BookingController : Controller 
{
    private readonly ApplicationDbContext _dbContext;

    public BookingController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("{id}")]
    // [Authorize(Roles = "ROLE_ADMIN")]
    public async Task<IActionResult> Get(int id)
    {
        var booking = await _dbContext.Bookings.FindAsync(id);
        if (booking == null) return NotFound(new { message = "Booking not found" });
        return Ok(booking);
    }

    [HttpGet("list-booking")]
    [Authorize(Roles = "ROLE_ADMIN")]
    public async Task<IActionResult> All()
    {
        var bookings = await _dbContext.Bookings.ToListAsync();
        return Ok(bookings);
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
    public async Task<IActionResult> Create([FromBody] BookingViewModel bookingViewModel)
    {
        if (bookingViewModel == null)
        {
            return BadRequest(new { message = "Booking data is required." });
        }
        var days = (bookingViewModel.date_check_out - bookingViewModel.date_check_in).TotalDays;
        if (days <= 0)
        {
            return BadRequest(new { message = "Ngày check out phải sau ngày check in." });
        }
        var home = await _dbContext.Bookings.FindAsync(bookingViewModel.home_id);

        await _dbContext.Bookings.AddAsync(new Booking {
            user_id = bookingViewModel.user_id,
            home_id = bookingViewModel.home_id,
            date_check_in = bookingViewModel.date_check_in,
            date_check_out = bookingViewModel.date_check_out,
            total_price = (int)(days * home.total_price),
        });
        await _dbContext.SaveChangesAsync();

        var booking = await _dbContext.Bookings.OrderByDescending(t => t.booking_id) 
                                   .FirstOrDefaultAsync();
        return Ok(booking);
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