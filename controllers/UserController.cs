using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ispk.data;
using ispk.models;
using ispk.dto;

namespace ispk.controllers {
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase {
	private readonly AppDbContext _db;

        public UsersController(AppDbContext db) {
            _db = db;
        }

	[HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers() {
            var users = await _db.Users.ToListAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id) {
            var user = await _db.Users.FindAsync(id);
            if (user == null) {
		return NotFound();
	    } 
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<User>> GetUser(UserDTO userDTO) {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.email == userDTO.email && u.password == userDTO.password);
            if (user == null) {
		return NotFound();
	    }
            return Ok(user);
        }

	[HttpPost("signup")]
        public async Task<ActionResult<User>> CreateUser(User user) {
	    try {
		_db.Users.Add(user);
		await _db.SaveChangesAsync();
		return CreatedAtAction(nameof(GetUser), new { id = user.id }, user);
	    } catch {
		return StatusCode(500, "Something whent wrong");
	    }
        }
    }
}
