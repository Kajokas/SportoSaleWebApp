using System.IdentityModel.Tokens.Jwt;
using System.Text;

using ispk.data;
using ispk.dto;
using ispk.models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ispk.controllers {
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _db;

        public UsersController(
                UserManager<User> userManager,
                SignInManager<User> signInManager,
                IConfiguration configuration,
                AppDbContext db) {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> getUsers() {
            var users = await _db.Users.ToListAsync();
            return Ok(users.Select(u => new { u.Id, u.UserName, u.Email }));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> getUser(int id) {
            var user = await _db.Users.FindAsync(id);
            if (user == null) {
                return NotFound();
            }
            return Ok(new { user.Id, user.UserName, user.Email });
        }

        [HttpPost("signup")]
        public async Task<ActionResult> signup([FromBody] UserDTO userDTO) {
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
        public async Task<ActionResult> login([FromBody] UserDTO userDTO) {
            var user = await _userManager.FindByEmailAsync(userDTO.email!);
            if (user == null) {
                return Unauthorized("Invalid email or password");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, userDTO.password!, false);
            if (!result.Succeeded) {
                return Unauthorized("Invalid email or password");
            }

            var response = new UserDTO {
                email = user.Email,
                name = user.UserName,
                role = user.role
            };

            var token = generateAccessToken(response.name!);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            Response.Cookies.Append("AccessToken", tokenString, new CookieOptions {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(6000)
            });

            return Ok(new { message = "Login successful", response });
        }

        [HttpPut("update")]
        public async Task<ActionResult> updateUser([FromBody] UserDTO newUser) {
            var user = await _userManager.FindByEmailAsync(newUser.email!);
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

        [HttpGet("logout")]
        public ActionResult logOut() {
            Response.Cookies.Delete("AccessToken");

            return Ok(new { message = "Logged out successfully" });
        }

        private JwtSecurityToken generateAccessToken(string userName) {
            var token = new JwtSecurityToken(
                    issuer: _configuration["JwtSettings:Issuer"],
                    audience: _configuration["JwtSettings:Audience"],
                    expires: DateTime.UtcNow.AddMinutes(1),
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]!)),
                        SecurityAlgorithms.HmacSha256)
                    );

            return token;
        }
    }
}
