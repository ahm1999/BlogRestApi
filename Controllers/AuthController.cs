
using BlogAPI.Dtos;
using BlogAPI.Models;
using BlogAPI.Services;
using BlogAPI.utils;
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
        public AuthController(AppDbContext context,ItokenService token)
        {
            _context = context;
            _token = token;
        }
        [HttpPost("login")]
        public async Task<IActionResult> LogIn ([FromBody] LoginDTO userData){
            var user = await _context.Users.FirstOrDefaultAsync(u => u.email == userData.email && u.password == userData.password);
            if (user == null){
                return BadRequest("no user with that email");
            }

            var token =  _token.CreateToken(user);


            return Ok(token);
        }
        

        [HttpPost("register")]
        public async  Task<IActionResult> Register ([FromBody] RegisterDTO userData){

            if (await _context.Users.AnyAsync(u => u.email ==userData.email ))
            {
                return BadRequest("A user Already registererd with that email");
                
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
            return Ok("user created");
        }
    }
}