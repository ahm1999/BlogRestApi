
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
    public class CommentsController : ControllerBase
    {   
        private readonly AppDbContext _context;

        private readonly IUserManger _userManeger;
        //private readonly ILogger<CommentsController> _logger; 

        public CommentsController(AppDbContext context, IUserManger userManeger )
        {
                _context = context;
                _userManeger = userManeger;
        }
        
        [Authorize]
        [HttpPost("add/{PostId:Guid}")]
        public async Task<IActionResult> Comment([FromRoute] Guid PostId,[FromBody] CommentDTO userData ){

            if (!await  _context.Posts.AnyAsync(p => p.Id == PostId )){
                return NotFound();
            }
            Comment newComment = new Comment (){
                Id = Guid.NewGuid(),
                PostId = PostId,
                content = userData.content,
                UserId = Guid.Parse(_userManeger.GeUserId()),
                UserName = _userManeger.GetUserName() 

            };

            await _context.Comments.AddAsync(newComment);
            await _context.SaveChangesAsync();
            return Ok("comment created");
        } 
        
    }
}