using LuxStayApi.Data;
using LuxStayApi.Models;
using LuxStayApi.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LuxStayApi.Controllers.Api;

[Route("api/[controller]")]
public class UserController : Controller 
{
    private readonly ApplicationDbContext _dbContext;

    public UserController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("{id}")]
    // [Authorize(Roles = "ROLE_ADMIN")]
    public async Task<IActionResult> Get(int id)
    {
        var user = await _dbContext.Users.FindAsync(id);
        if (user == null) return NotFound(new { message = "User not found" });
        return Ok(user);
    }

    [HttpGet("list-user")]
    // [Authorize(Roles = "ROLE_ADMIN")]
    public async Task<IActionResult> All()
    {
        var users = await _dbContext.Users.ToListAsync();
        return Ok(users);
    }


    [HttpPost("delete/{id}")]
    // [Authorize(Roles = "ROLE_ADMIN")]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _dbContext.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound(new { message = "User not found" });
        }

        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
        return Ok(new { 
            message = "Xóa thành công UwU", 
            user = user, 
        });
    }


    [HttpPost("create")]
    // [Authorize(Roles = "ROLE_ADMIN")]
    public async Task<IActionResult> Create([FromBody] UserViewModel userViewModel)
    {
        if (userViewModel == null)
        {
            return BadRequest(new { message = "User data is required." });
        }
        var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.email == userViewModel.email);
        if (existingUser != null)
        {
            return BadRequest(new { message = "User already exists." });
        }


        await _dbContext.Users.AddAsync(new User {
            email = userViewModel.email,
            phone = userViewModel.phone,
            name = userViewModel.name,
            password = userViewModel.password,
            role = userViewModel.role ?? "ROLE_USER",
            address = userViewModel.address ?? "",
            gender = userViewModel.gender,
            verify = userViewModel.verify
        });
        await _dbContext.SaveChangesAsync();

        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.email == userViewModel.email);

        return CreatedAtAction(nameof(Get), new { id = user?.user_id }, user);
    }


    [HttpPost("update/{id}")]
    // [Authorize(Roles = "ROLE_ADMIN")]
    public async Task<IActionResult> Update(int id, [FromBody] UserViewModel userViewModel)
    {
        if (userViewModel == null)
        {
            return BadRequest(new { message = "User data is invalid." });
        }

        var existingUser = await _dbContext.Users.FindAsync(id);
        if (existingUser == null)
        {
            return NotFound(new { message = "User not found" });
        }

        existingUser.name = userViewModel.name;
        existingUser.phone = userViewModel.phone;
        existingUser.email = userViewModel.email;
        existingUser.address = userViewModel.address ?? "";
        existingUser.gender = userViewModel.gender;
        existingUser.role = userViewModel.role ?? "ROLE_USER";

        await _dbContext.SaveChangesAsync();
        return Ok(new { 
            message = "Cập nhật thành công UwU", 
            user = existingUser, 
        });
    }

}