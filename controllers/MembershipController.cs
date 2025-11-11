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
        public async Task<ActionResult<IEnumerable<Membership>>> getMembership() {
            var memberships = await _db.Membership.ToListAsync();
            return Ok(memberships.Select(m => new { m.id, m.createdAt, m.expirationDate}));
        }
    }
}
