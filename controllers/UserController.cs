using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ispk.data;
using ispk.models;
using ispk.dto;

namespace ispk.controllers {
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
	private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly AppDbContext _db;

        public UsersController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole<int>> roleManager, AppDbContext db) {
            _userManager = userManager;
            _signInManager = signInManager;
	    _roleManager = roleManager;
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers() {
            var users = await _db.Users.ToListAsync();
            return Ok(users.Select(u => new { u.Id, u.UserName, u.Email }));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id) {
            var user = await _db.Users.FindAsync(id);
            if (user == null) {
		return NotFound();
	    }
            return Ok(new { user.Id, user.UserName, user.Email});
        }

        [HttpPost("signup")]
        public async Task<ActionResult> Signup([FromBody] UserDTO userDTO) {
            var existing = await _userManager.FindByEmailAsync(userDTO.email!);
            if (existing != null) {
		return BadRequest("Email already registered");
	    }

            var user = new User {
                Email = userDTO.email,
                UserName = userDTO.name,
		role = userDTO.role
            };

            var result = await _userManager.CreateAsync(user, userDTO.password!);
            if (!result.Succeeded) {
                return BadRequest(result.Errors);
	    }

            return Ok(new { message = "User created", user.Id, user.Email });
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UserDTO userDTO) {
            var user = await _userManager.FindByEmailAsync(userDTO.email);
	    if (user == null) {
		return Unauthorized("Invalid email or password");
	    }

            var result = await _signInManager.CheckPasswordSignInAsync(user, userDTO.password, false);
            if (!result.Succeeded) {
		return Unauthorized("Invalid email or password");
	    }

	    var response = new UserDTO {
		email = user.Email,
		name = user.UserName,
		role = user.role
	    };
            return Ok(new { message = "Login successful", response});
        }

        [HttpPut("update")]
        public async Task<ActionResult> UpdateUser([FromBody] UserDTO newUser) {
            var user = await _userManager.FindByEmailAsync(newUser.email);
            if (user == null) {
		return NotFound();
	    }

            user.UserName = newUser.name;
            user.Email = newUser.email;
	    user.role = newUser.role;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) {
                return BadRequest(result.Errors);
	    }

            return Ok(new { message = "User updated", user.Id, user.UserName, user.Email });
        }
    }
}

