using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using my_api_project.Data;
using my_api_project.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace my_api_project.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public LoginController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // POST: /Login
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var cardHolder = await _context.CardHolders
                .FirstOrDefaultAsync(c =>
                    c.UserName == request.UserName &&
                    c.Password == request.Password);

            if (cardHolder == null)
            {
                return Unauthorized("Invalid credentials.");
            }

            // Create JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("your_super_secret_key_here"); // Use the same key as in Startup
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, cardHolder.CardHolderId.ToString()),
                    new Claim(ClaimTypes.Name, cardHolder.UserName ?? "")
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token),
                cardholder = new
                {
                    cardHolderId = cardHolder.CardHolderId,
                    userName = cardHolder.UserName,
                    first = cardHolder.First,
                    last = cardHolder.Last
                }
            });
        }
    }

    public class LoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}