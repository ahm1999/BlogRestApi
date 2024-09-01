
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogAPI.Models
{
    public class Post
    {
        public Guid Id { get; set; }
        public String Title { get; set; } = String.Empty;
        
        public String Content { get; set; } = String.Empty;

        public DateTime AddedOn { get; set; }

        [ForeignKey("User")]
        public Guid UserId { get; set; } 

        public User User { get; set; }

        [ForeignKey(nameof(Blog))]
        public Guid BlogId { get; set; }

        public Blog ParentBlog { get; set; }  

        public ICollection<Comment> Comments {get ; set ; }


    }
}