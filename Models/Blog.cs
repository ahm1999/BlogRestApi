using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogAPI.Models
{
    public class Blog
    {
        public Guid Id { get; set; }

        public string title { get; set; }

        public string Desciption { get; set; }

        public bool Personal { get; set; }

        [ForeignKey(nameof(User))]
        public Guid CreatorId { get; set; }

        public User Creator { get; set; }

        public DateTime CreatedOn { get; set; }

        public  ICollection<Post> Posts { get; set; }
    }
}
