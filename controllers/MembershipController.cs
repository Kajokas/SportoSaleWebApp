using System.IdentityModel.Tokens.Jwt;
using System.Text;

using Microsoft.AspNetCore.Authorization;

using ispk.data;
using ispk.dto;
using ispk.models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ispk.controllers {
    [ApiController]
    [Route("membership")]
    public class MembershipController: ControllerBase {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _db;

        public MembershipController (
                IConfiguration configuration,
                AppDbContext db) {
            _configuration = configuration;
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MembershipDTO>>> getMembership() {
            var memberships = await _db.Membership.ToListAsync();
	    var membershipsDTOs = memberships
		.Select(m => MembershipDTO.createDtoFromMembership(m))
		.ToList();

	    return Ok(membershipsDTOs);
        }

	[HttpGet("user/{id}")]
	public async Task<ActionResult<MembershipDTO>> getUserMembership(int id) {
	    var membership = await _db.Membership
		.Include(m => m.user)
		.Include(m => m.membershipType)
		.FirstOrDefaultAsync(m => m.userId == id);

	    var dto = membership != null ? MembershipDTO.createDtoFromMembership(membership) : null;

	    return Ok(dto);
	}

	[HttpPost("purchase")]
	public async Task<ActionResult<MembershipDTO>> purchaseMembership() {
	    int userId; 
	    try {
		userId = authorize();
	    } catch {
		return Unauthorized("You need to be loged in to make a purchase");
	    }

	    //TODO: change to actual choice
	    var membershipTypeId = 1;

	    var membershipType = await _db.MembershipType.FindAsync(membershipTypeId);

	    var membership = createMembership(userId, membershipType);

	    _db.Membership.Add(membership);
	    await _db.SaveChangesAsync(); 

	    return Ok("Done");
	}

	[HttpDelete("cancel/{id}")]
	public async Task<ActionResult<string>> deleteMembership(int id) {

	    var membership = await _db.Membership.FindAsync(id);
	    if (membership != null) {
		_db.Membership.Remove(membership);
		await _db.SaveChangesAsync();
	    }

	    return Ok("Memebership has been canceled");
	}

	private int authorize() {
	    var token = Request.Cookies["AccessToken"];

	    var handler = new JwtSecurityTokenHandler();
	    var jwtToken = handler.ReadJwtToken(token);
	    var claims = jwtToken.Claims;

	    var userIdClaim = claims.FirstOrDefault(c => c.Type == "userId")?.Value;

	    var userId = int.Parse(userIdClaim);

	    return userId;
	}

	private Membership createMembership(int userId, MembershipType membershipType) {
	    var membership = new Membership {
		createdAt = DateTime.Now,
		expirationDate = DateTime.Now + membershipType.validity,
		userId = userId,
		membershipTypeId = membershipType.id
	    };

	    return membership;
	}
    }
}
