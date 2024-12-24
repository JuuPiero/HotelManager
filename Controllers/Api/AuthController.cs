using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LuxStayApi.Data;
using LuxStayApi.Models;
using LuxStayApi.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace LuxStayApi.Controllers.Api;

public class AuthController : Controller 
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IConfiguration _config;  
    
    public AuthController(ApplicationDbContext dbContext, IConfiguration config)
    {
        _dbContext = dbContext;
        _config = config;
    }

    [HttpPost("api/signup")]
    public async Task<IActionResult> Signup([FromBody] SignupRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); 
        }
        var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.email == request.email);
        if (existingUser != null)
        {
            return BadRequest(new { message = "Email already exists." });
        }

        await _dbContext.Users
            .AddAsync(new User {
                email = request.email,
                phone = request.phone,
                name = request.name,
                password = request.password,
                role = "ROLE_USER",
                verify = true
            });
        await _dbContext.SaveChangesAsync();

        return Ok(new { 
            message = "Đăng ký thành công", 
            user = request.email,
        });
    }

    [HttpPost("api/login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); // Trả về lỗi validate
        }

        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.email == request.email && u.password == request.password);

        if (user == null)
        {
            return Unauthorized(new { message = "Thông tin không chính xác." });
        }
        var token = GenerateJwtToken(user);

        return Ok(new { 
            message = "Đăng nhập thành công", 
            user = user, 
            token = token 
        });
    }

    private string GenerateJwtToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.name),
            new Claim(JwtRegisteredClaimNames.Email, user.email),
            new Claim(ClaimTypes.Role, user.role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_config["Jwt:TokenExpirationInMinutes"])),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}