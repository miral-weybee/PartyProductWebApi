using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PartyProductWebApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PartyProductWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly EvaluationTaskDbContext _context;

        public LoginController(EvaluationTaskDbContext _context)
        {
            this._context = _context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(Login user)
        {
            var existingUser = await _context.Logins.FirstOrDefaultAsync(u => u.Username == user.Username);

            if (existingUser == null || existingUser.Password != user.Password)
            {
                return Unauthorized();
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("partyproductAPIkey1234567890123456");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.Name, existingUser.Username),
        }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);


            return Ok(new { Token = tokenString, });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(Login user)
        {

            _context.Logins.Add(user);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
