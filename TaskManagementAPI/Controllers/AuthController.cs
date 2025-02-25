using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLogin userLogin)
        {
            try
            {
                // Validate user credentials
                if (userLogin.Username == "username" && userLogin.Password == "password") // Replace with real validation logic from db
                {
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.Name, userLogin.Username),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique identifier for the token
                        new Claim(ClaimTypes.Role, "User")  // You can add roles or other claims as needed
            };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
                    var token = new JwtSecurityToken(
                                    issuer: _configuration["Jwt:Issuer"],
                                    audience: _configuration["Jwt:Audience"],
                                    claims: claims,
                                    expires: DateTime.Now.AddHours(1),  // Token expiry time
                                    signingCredentials: creds
                                );
                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token)
                    });
                }
                return Unauthorized(); // Invalid credentials
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
