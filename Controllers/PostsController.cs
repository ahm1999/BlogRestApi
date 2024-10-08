
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
        [HttpPost("AddPost/{blogId:guid}")]
        public async Task<IActionResult> CreatePost([FromRoute] Guid blogId, [FromBody] PostDto_Req userData)
        {
            Guid userId = Guid.Parse(_userManeger.GeUserId());
        
            var blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == blogId);

            if(blog == null ) return BadRequest("no blog with that id");

            if (blog.Personal && userId != blog.CreatorId) return Unauthorized("Personal Blog");

            Guid retId = Guid.NewGuid();
            Post createdPost = new Post()
            {
                Id = retId,
                Title = userData.Title,
                Content = userData.Content,
                BlogId = blogId,
                UserId = userId,
                AddedOn = DateTime.UtcNow
            };
            await _context.Posts.AddAsync(createdPost);
            await _context.SaveChangesAsync();
            return Ok(retId.ToString());

        }


        [HttpGet("All")]
        public async Task<IActionResult> GetAll([FromQuery] int positoin)
        {
          
            var Posts = await _context.Posts
                                .OrderByDescending(p => p.AddedOn)
                                .Skip(positoin*10)
                                .Take(10)
                                .ToListAsync();


            return Ok(Posts);
        }

        [HttpGet("Count")]
        public async Task<IActionResult> GetAllPostCount() {
            int count = await _context.Posts.CountAsync();

            return Ok( new { Count = count });
        
        }

        [HttpGet("Count/{BlogId:guid}")]

        public async Task<IActionResult> GetPostsCountInBlog([FromRoute] Guid BlogId) {
            int count = await _context.Posts.CountAsync(p => p.BlogId == BlogId);

            return Ok(new {Count = count});
        }

        [HttpGet("Blog/{BlogId:guid}")]
        public async Task<IActionResult> GetAllInBlog([FromRoute] Guid BlogId,[FromQuery] int positoin)
        {
            
            var Posts = await _context.Posts
                                .OrderByDescending(p => p.AddedOn)
                                .Skip(positoin * 10)
                                .Take(10)
                                .Where(p => p.BlogId == BlogId)
                                .ToListAsync();


            return Ok(Posts);
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