using MainProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MainProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController(JwtOptions jwtOptions, ApplicationDbContext dbContext) : ControllerBase
    {
        [HttpPost]
        [Route("auth")]
        public ActionResult<string> AuthenticateUser(AuthenticationRequest request)
        {
            var user = dbContext.Set<User>().FirstOrDefault(x => x.Name == request.userName && 
            x.Password == request.Password);
            if (user == null) 
                return Unauthorized();
            var tokenHandler = new JwtSecurityTokenHandler();
            var Descriptor = new SecurityTokenDescriptor
            {
                Issuer = jwtOptions.Issuer,
                Audience = jwtOptions.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Signingkey)),
                SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new(ClaimTypes.Name, user.Name),
                    new(ClaimTypes.Role, "Admin"),
                    new("UserType", "Employee")
                })
            };
            var securityToken = tokenHandler.CreateToken(Descriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);
            return Ok(accessToken);
        }
    }
}
