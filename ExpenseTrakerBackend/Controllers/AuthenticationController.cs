using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PracticeCrud.Common;
using PracticeCrud.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PracticeCrud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public AuthenticationController(IConfiguration configuration)
        {
             _configuration = configuration;
        }


        [HttpPost]

        public ActionResult Login([FromBody] User userlogin) {

            var user = Authenciate(userlogin);
            if (user != null) {

                var token = GenerateToken(user);
                return Ok(token);
            }
            return NotFound("user not found");

        }

        private string GenerateToken(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                 new Claim(ClaimTypes.NameIdentifier,user.Username),
                 new Claim(ClaimTypes.Role,user.Role)
            };
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UserModel Authenciate(User user) { 
         var currentuser=UserConstants.Users.FirstOrDefault(x=>x.Username.ToLower()==user.Username.ToLower() && x.Password==user.Password);

            if (currentuser != null) { 
              return currentuser;
            }
            return null;
        }

    }
}
