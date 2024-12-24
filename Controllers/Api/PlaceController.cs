using LuxStayApi.Data;
using LuxStayApi.Models;
using LuxStayApi.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LuxStayApi.Controllers.Api;

[Route("api/[controller]")]
public class PlaceController : Controller 
{
    private readonly ApplicationDbContext _dbContext;

    public PlaceController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var place = await _dbContext.Places.FindAsync(id);
        if (place == null) return NotFound(new { message = "Place not found" });
        var homes = await _dbContext.Homes.Where(h => h.place_id == place.place_id).ToListAsync(); 
        var tours = await _dbContext.Tours.Where(t => t.place_id == place.place_id).ToListAsync();
        return Ok(new {
            place = place,
            homes = homes,
            tours = tours,
        });
    }

    [HttpGet("list-place")]
    public async Task<IActionResult> All()
    {
        var places = await _dbContext.Places.ToListAsync();
        return Ok(places);
    }


    [HttpPost("delete/{id}")]
    [Authorize(Roles = "ROLE_ADMIN")]
    public async Task<IActionResult> Delete(string id)
    {
        var place = await _dbContext.Places.FirstOrDefaultAsync(p => p.place_id == id);
        if (place == null)
        {
            return NotFound(new { message = "Place not found" });
        }

        _dbContext.Places.Remove(place);
        await _dbContext.SaveChangesAsync();
        return Ok(new { 
            message = "Xóa thành công UwU", 
            place = place, 
        });
    }


    [HttpPost("create")]
    [Authorize(Roles = "ROLE_ADMIN")]
    public async Task<IActionResult> Create([FromBody] PlaceViewModel placeViewModel)
    {
        if (placeViewModel == null)
        {
            return BadRequest(new { message = "Place data is required." });
        }
        var existingPlace = await _dbContext.Places.FirstOrDefaultAsync(p => p.place_id == placeViewModel.place_id);
        if (existingPlace != null)
        {
            return BadRequest(new { message = "Place already exists." });
        }

        await _dbContext.Places.AddAsync(new Place {
            place_id = placeViewModel.place_id,
            place_name = placeViewModel.place_name,
            image = placeViewModel.image,
            total_home = placeViewModel.total_home
        });
        await _dbContext.SaveChangesAsync();

        var place = await _dbContext.Places.FirstOrDefaultAsync(p => p.place_id == placeViewModel.place_id);

        return CreatedAtAction(nameof(Get), new { id = place?.place_id }, place);
    }


    [HttpPost("update/{id}")]
    [Authorize(Roles = "ROLE_ADMIN")]
    public async Task<IActionResult> Update(string id, [FromBody] PlaceViewModel placeViewModel)
    {
        if (placeViewModel == null)
        {
            return BadRequest(new { message = "Place data is invalid." });
        }

        var existingPlace = await _dbContext.Places.FirstOrDefaultAsync(p => p.place_id == id);
        if (existingPlace == null)
        {
            return NotFound(new { message = "Place not found" });
        }

        existingPlace.place_name = placeViewModel.place_name;
        existingPlace.total_home = placeViewModel.total_home;
        existingPlace.image = placeViewModel.image;

        await _dbContext.SaveChangesAsync();
        return Ok( new { 
            message = "Cập nhật thành công UwU", 
            place = existingPlace, 
        });
    }

}