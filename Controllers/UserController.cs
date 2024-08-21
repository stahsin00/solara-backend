using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Solara.Data;
using Solara.Models;

namespace Solara.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationContext _context;

        private readonly ILogger<UserController> _logger;

        public UserController(ApplicationContext context, ILogger<UserController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET /api/user
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUser()
        {
            try {
                var email = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(email))
                {
                    return BadRequest("Invalid email claim.");
                }

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    return NotFound(new { message = "User not found." });
                }

                return Ok(user);
            } catch (Exception e) {
                _logger.LogError(e, "Error in UserController - GetUser:");
                return StatusCode(500, new { message = "Unable to get user information." });
            }
        }
    }
}
