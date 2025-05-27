using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using my_api_project.Data;
using my_api_project.Models;
using System.Threading.Tasks;

namespace my_api_project.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;
        }

        // POST: /Login
        [HttpPost]
        public async Task<ActionResult<CardHolder>> Login([FromBody] LoginRequest request)
        {
            var cardHolder = await _context.CardHolders
                .FirstOrDefaultAsync(c =>
                    c.First == request.First &&
                    c.Last == request.Last &&
                    c.Dob == request.Dob);

            if (cardHolder == null)
            {
                return Unauthorized("Invalid credentials.");
            }

            return Ok(cardHolder);
        }
    }

    public class LoginRequest
    {
        public string First { get; set; }
        public string Last { get; set; }
        public string Dob { get; set; }
    }
}