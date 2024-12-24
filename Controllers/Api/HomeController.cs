using LuxStayApi.Data;
using LuxStayApi.Models;
using LuxStayApi.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LuxStayApi.Controllers.Api;

[Route("api/[controller]")]
public class HomeController : Controller 
{
    private readonly ApplicationDbContext _dbContext;

    public HomeController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var home = await _dbContext.Homes.FindAsync(id);
        if (home == null) return NotFound(new { message = "Home not found" });
        return Ok(home);
    }

    [HttpGet("list-home")]
    public async Task<IActionResult> All()
    {
        var homes = await _dbContext.Homes.ToListAsync();
        return Ok(homes);
    }


    [HttpPost("delete/{id}")]
    [Authorize(Roles = "ROLE_ADMIN")]
    public async Task<IActionResult> Delete(int id)
    {
        var home = await _dbContext.Homes.FindAsync(id);
        if (home == null)
        {
            return NotFound(new { message = "Home not found" });
        }

        _dbContext.Homes.Remove(home);
        await _dbContext.SaveChangesAsync();
        return Ok(new { 
            message = "Xóa thành công UwU", 
            home = home, 
        });
    }


    [HttpPost("create")]
    [Authorize(Roles = "ROLE_ADMIN")]
    public async Task<IActionResult> Create([FromBody] HomeViewModel homeViewModel)
    {
        if (homeViewModel == null)
        {
            return BadRequest(new { message = "User data is required." });
        }

        await _dbContext.Homes.AddAsync(new Home {
            home_name = homeViewModel.home_name,
            home_type = homeViewModel.home_type,
            room_number = homeViewModel.room_number,
            price = homeViewModel.price,
            place_id = homeViewModel.place_id,
            image_intro = homeViewModel.image_intro,
            address = homeViewModel.address,
            short_description = homeViewModel.short_description,
            detail_description = homeViewModel.detail_description,
            restore = homeViewModel.restore
        });
        await _dbContext.SaveChangesAsync();

        var home = await _dbContext.Homes.OrderByDescending(home => home.home_id)  // Sắp xếp theo ID giảm dần
                                   .FirstOrDefaultAsync();

        return CreatedAtAction(nameof(Get), new { id = home?.home_id }, home);
    }


    [HttpPost("update/{id}")]
    [Authorize(Roles = "ROLE_ADMIN")]
    public async Task<IActionResult> Update(int id, [FromBody] HomeViewModel homeViewModel)
    {
        if (homeViewModel == null)
        {
            return BadRequest(new { message = "Home data is invalid." });
        }

        var existingHome = await _dbContext.Homes.FindAsync(id);
        if (existingHome == null)
        {
            return NotFound(new { message = "Home not found" });
        }

        existingHome.home_name = homeViewModel.home_name;
        existingHome.home_type = homeViewModel.home_type;
        existingHome.room_number = homeViewModel.room_number;
        existingHome.price = homeViewModel.price;
        existingHome.place_id = homeViewModel.place_id;
        existingHome.image_intro = homeViewModel.image_intro;
        existingHome.address = homeViewModel.address;
        existingHome.short_description = homeViewModel.short_description;
        existingHome.detail_description = homeViewModel.detail_description;

        await _dbContext.SaveChangesAsync();
        return Ok( new { 
            message = "Cập nhật thành công UwU", 
            home = existingHome, 
        });
    }

}