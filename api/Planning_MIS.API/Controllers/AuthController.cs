using Domain.Entities.User;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Planning_MIS.API.Services;
using Planning_MIS.API.ViewModels.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Planning_MIS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUnitOfWork _repo;
        private readonly IConfiguration _configuration;

        public AuthController(IUnitOfWork repo, IConfiguration configuration)
        {
            _repo = repo;
            _configuration = configuration;
        }

        [HttpGet("login-office-details")]
        public async Task<IActionResult> OfficeLoginDetail() 
        { 
            var office = await _repo.Setup.GetOfficeInfo();
            var o = office.FirstOrDefault();
            return Ok(new{
                o.MainHeading,
                o.SubHeading3,
                o.Name,
                o.Address1,
                o.Address2,
                o.Photo,
                o.Footer
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest(new { message = "Username and password are required" });
            }

            var user = await _repo.Users.GetByUsernameAsync(model.Username);
            if (user == null)
                return Unauthorized("Invalid username or password");

            bool isPasswordValid = PasswordHelper.VerifyPassword(model.Password, user.Password, user.Salt);
            if (!isPasswordValid)
                return Unauthorized("Invalid username or password");

            var wardIds = user.WardId;
            var wardIdsJson = JsonSerializer.Serialize(wardIds);
            var fiscalYear = await _repo.Setup.GetCurrentFiscalYear();

            var claims = new List<Claim>
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),

            new Claim("Username", user.UserName),
            new Claim("UserTypeId", user.UserTypeId.ToString()),
            new Claim("DepartmentId", user.DepartmentId.ToString()),
            new Claim("IsDepartmentUser", user.IsDepartmentUser.ToString()),
            new Claim("WardIds", wardIdsJson),
            new Claim("FiscalYearId", fiscalYear.Id.ToString())
            };

            


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new
            {
                token = tokenString,
                user = user.UserName,
                expiration = token.ValidTo
            }); 
        }



        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Handle logout logic
            // invalidate the token or clear session
            return Ok(new { message = "Logged out successfully" });
        }


        [HttpGet("verify")]
        public IActionResult VerifyToken()
        {
            // used by frontend to verify if token is still valid
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "Invalid or expired token" });
            }

            return Ok(new
            {
                valid = true,
                userId = userId,
                username = User.FindFirst("Username")?.Value
            });
        }
    }
}
