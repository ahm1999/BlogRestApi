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
                CreatorId = Guid.Parse(_userManeger.GeUserId())
            };
            await _context.Blogs.AddAsync(blog);
            await _context.SaveChangesAsync();
            return Ok($"Blog with the Id {BlogId.ToString()}");
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

    }
}
