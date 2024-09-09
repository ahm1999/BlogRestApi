using BlogAPI.Dtos;
using BlogAPI.Models;
using BlogAPI.Services;
using BlogAPI.utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IUserManger _userManeger;
        public BlogController(AppDbContext context , IUserManger userManger)
        {
            _context = context;
            _userManeger = userManger;
            
        }

        [HttpPost("CreateBlog")]
        [Authorize]
        public async Task<IActionResult> CreateBlog(CreateBlogDTO userData) {
            var BlogId = Guid.NewGuid();

            Blog blog = new Blog()
            {
                Id = BlogId,
                Desciption = userData.Desciption,
                title = userData.title,
                CreatorId = Guid.Parse(_userManeger.GeUserId()),
                Personal = userData.Personal,
                CreatedOn = DateTime.UtcNow

            };
            await _context.Blogs.AddAsync(blog);
            await _context.SaveChangesAsync();
            return Ok(new {status = "success" ,BlogId=BlogId.ToString()  });
          }

        
        [Authorize]
        [HttpDelete("delete/{blogId:guid}")]
        public async Task<IActionResult> DeleteBlog(Guid blogId)
        {
            var blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == blogId);

            if (blog == null) return BadRequest("No Blog with that Id");

            if (blog.CreatorId != Guid.Parse(_userManeger.GeUserId())) return Unauthorized();

            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();
            return Ok("Blog Deleted !");



        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAllBlogs([FromQuery] int positoin) {
            var _Blogs = await _context.Blogs
                               .OrderByDescending(p => p.CreatedOn)
                               .Skip(positoin * 10)
                               .Take(10)
                               .ToListAsync();
            return Ok(new { status= "success", Blogs =  _Blogs});
        }


        [HttpGet("Count")]
        public async Task<IActionResult> GetAllBlogCount() {
            int count = await _context.Blogs.CountAsync();

            return Ok(new {Count = count });
        
        }




    }
}
