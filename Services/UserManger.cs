using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BlogAPI.Services
{
    
    public class UserManger : IUserManger
    {

        private readonly IHttpContextAccessor _contextAccessor;
        public UserManger(IHttpContextAccessor contextAccessor)
            {
        _contextAccessor = contextAccessor;
            }
        public string GeUserId()
        {
            
            
            string Id = ReturnParsedToken().Claims.First(claim => claim.Type == "sub").Value;
            return Id;
        }
        private JwtSecurityToken ReturnParsedToken(){
            string Token =  _contextAccessor.HttpContext.Request.Headers.Authorization;
            Token = Token.Split()[1];
             var handler = new JwtSecurityTokenHandler();
             var ParsedToken =  handler.ReadToken(Token) as  JwtSecurityToken;
             return ParsedToken;
        }
        public string GetRole(){
            string Role = ReturnParsedToken().Claims.First(claim => claim.Type == "Roles").Value;
            return Role;
        }
        public string GetUserName(){
            string Name = ReturnParsedToken().Claims.First(claim => claim.Type == "nameid").Value;
            return Name;
        }
    }
}