
using BlogAPI.Dtos;
using BlogAPI.Models;
using BlogAPI.Services;
using BlogAPI.utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {   
        private readonly AppDbContext _context ;
        private readonly ItokenService _token ;
        private readonly IUserManger _userManeger;
        public AuthController(AppDbContext context,ItokenService token, IUserManger userManeger)
        {
            _context = context;
            _token = token;
            _userManeger = userManeger;
        }
        [HttpPost("login")]
        public async Task<IActionResult> LogIn ([FromBody] LoginDTO userData){
            var user = await _context.Users.FirstOrDefaultAsync(u => u.email == userData.email && u.password == userData.password);
            if (user == null){
                return BadRequest(new {status=StatusCodes.Status400BadRequest, title ="no user with that email"});
            }

            var token =  _token.CreateToken(user);

            UserResponseDTO response = new UserResponseDTO() { 
            Id = user.Id,
            email = user.email,
            Name = user.Name,
            Role = user.Role
            };

            return Ok(new {status="Success",User = response ,token=token});
        }

        [Authorize]
        [HttpGet("CurrentUser")]
        public async Task<IActionResult> GetCurrentUser() {
            var user =  await _context.Users.FirstOrDefaultAsync(u => u.Id ==Guid.Parse(_userManeger.GeUserId()));
            UserResponseDTO response = new UserResponseDTO()
            {
                Id = user.Id,
                email = user.email,
                Name = user.Name,
                Role = user.Role
            };

            return Ok(new { status = "success", User = response });
        }

        

        [HttpPost("register")]
        public async  Task<IActionResult> Register ([FromBody] RegisterDTO userData){

            if (await _context.Users.AnyAsync(u => u.email ==userData.email ))
            {
                return BadRequest(new {status=StatusCodes.Status400BadRequest,title ="A user Already registererd with that email"});
                
            }
            
            User user = new User(){
                Id = Guid.NewGuid(),
                Name = userData.Name,
                email = userData.email,
                password = userData.password,
                Role = "User"
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return Ok(new {status=StatusCodes.Status201Created,title="user created"});
        }

        [Authorize]
        [HttpGet("CheckToken")]
        public IActionResult CheckToken(){
            return Ok(new {status = "success"});
        }
       
    }
}