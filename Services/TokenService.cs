

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BlogAPI.Models;
using Microsoft.IdentityModel.Tokens;

namespace BlogAPI.Services
{
    public class TokenService : ItokenService
    {
        private readonly IConfiguration _config;  
        public TokenService(IConfiguration config)
        {
            _config = config;
        }
        public string CreateToken(User user)
        {   
            var SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetValue<string>("JwtSecret")));
            var signingCreds = new SigningCredentials(SecurityKey,SecurityAlgorithms.HmacSha512);
            
            
            var Claims = new List<Claim>(){
                new Claim(JwtRegisteredClaimNames.NameId,user.Name),
                new Claim(JwtRegisteredClaimNames.Sub,user.Id.ToString()),
                new Claim(ClaimTypes.Role,user.Role)

            };

            var token = new JwtSecurityToken("Blogapi","client",Claims,expires:DateTime.Now.AddDays(7),signingCredentials:signingCreds);      
            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}