using LuxStayApi.Data;
using LuxStayApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LuxStayApi.Controllers.Api;

[Route("api/[controller]")]
public class TourController : Controller 
{
    private readonly ApplicationDbContext _dbContext;

    public TourController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var tour = await _dbContext.Tours.FindAsync(id);
        if (tour == null) return NotFound(new { message = "Tour not found" });
        return Ok(tour);
    }

    [HttpGet("list-tour")]
    public async Task<IActionResult> All()
    {
        var tours = await _dbContext.Tours.ToListAsync();
        return Ok(tours);
    }


    [HttpPost("delete/{id}")]
    // [Authorize(Roles = "ROLE_ADMIN")]
    public async Task<IActionResult> Delete(int id)
    {
        var tour = await _dbContext.Tours.FindAsync(id);
        if (tour == null)
        {
            return NotFound(new { message = "Tour not found" });
        }

        _dbContext.Tours.Remove(tour);
        await _dbContext.SaveChangesAsync();
        return Ok(new { 
            message = "Xóa thành công UwU", 
            tour = tour, 
        });
    }


    [HttpPost("create")]
    // [Authorize(Roles = "ROLE_ADMIN")]
    public async Task<IActionResult> Create([FromBody] TourViewModel tourViewModel)
    {
        if (tourViewModel == null)
        {
            return BadRequest(new { message = "Tour data is required." });
        }

        await _dbContext.Tours.AddAsync(new Tour {
            tour_name = tourViewModel.tour_name,
            tour_type = tourViewModel.tour_type,
            tour_number = tourViewModel.tour_number,
            price = tourViewModel.price,
            place_id = tourViewModel.place_id,
            image = tourViewModel.image ?? "",
            address = tourViewModel.address,
            short_description = tourViewModel.short_description,
            detail_description = tourViewModel.detail_description,
        });
        await _dbContext.SaveChangesAsync();

        var tour = await _dbContext.Tours.OrderByDescending(t => t.tour_id) 
                                   .FirstOrDefaultAsync();

        return CreatedAtAction(nameof(Get), new { id = tour?.tour_id }, tour);
    }


    [HttpPost("update/{id}")]
    // [Authorize(Roles = "ROLE_ADMIN")]
    public async Task<IActionResult> Update(int id, [FromBody] TourViewModel tourViewModel)
    {
        if (tourViewModel == null)
        {
            return BadRequest(new { message = "Tour data is invalid." });
        }

        var existingTour = await _dbContext.Tours.FindAsync(id);
        if (existingTour == null)
        {
            return NotFound(new { message = "Tour not found" });
        }

        existingTour.tour_name = tourViewModel.tour_name;
        existingTour.tour_type = tourViewModel.tour_type;
        existingTour.tour_number = tourViewModel.tour_number;
        existingTour.price = tourViewModel.price;
        existingTour.place_id = tourViewModel.place_id;
        existingTour.image = tourViewModel.image;
        existingTour.address = tourViewModel.address;
        existingTour.short_description = tourViewModel.short_description;
        existingTour.detail_description = tourViewModel.detail_description;

        await _dbContext.SaveChangesAsync();
        return Ok( new { 
            message = "Cập nhật thành công UwU", 
            tour = existingTour, 
        });
    }

}