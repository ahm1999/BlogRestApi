
using BlogAPI.Dtos;
using BlogAPI.Models;
using BlogAPI.Services;
using BlogAPI.utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace BlogAPI.Controllers
{   [ApiController]
    [Route("[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IUserManger _userManeger;

        public PostsController(AppDbContext context,IUserManger userManeger)
        {
            _context = context;
            _userManeger = userManeger;
        }

        [HttpGet("{Id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid Id )
        {   
           var Res = await _context.Posts
           .Include(p =>p.Comments)
           .FirstOrDefaultAsync (P => P.Id == Id);

            return Ok(Res);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] PostDto_Req userData){

                
                Guid retId = Guid.NewGuid();
                Post createdPost = new Post (){
                    Id = retId,
                    Title = userData.Title,
                    Content = userData.Content,
                    UserId = Guid.Parse(_userManeger.GeUserId())
                };
                await _context.Posts.AddAsync(createdPost);
                await _context.SaveChangesAsync();
                return Ok(retId.ToString());

        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {   

            return Ok( await _context.Posts.ToListAsync());
        }

        [Authorize]
        [HttpDelete("{Id:Guid}")]

        public async Task<IActionResult> DeletePost([FromRoute] Guid Id){
            Guid userId =Guid.Parse( _userManeger.GeUserId());
            Post post =  _context.Posts.FirstOrDefault(p => p.Id == Id && p.UserId == userId );
            if (post == null )
            {
                return NotFound();
            }
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return Ok("Post Deleted");

        }
        
        [Authorize(Roles ="admin")]
        [HttpDelete("admin/{Id:Guid}")]
       public async Task<IActionResult> DeletePostAdmin([FromRoute] Guid Id){
            Post post =  _context.Posts.FirstOrDefault(p => p.Id == Id  );
            if (post == null )
            {
                return NotFound();
            }
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return Ok("Post Deleted");

        }

     
    }
}