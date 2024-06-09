using System.ComponentModel.DataAnnotations.Schema;


namespace BlogAPI.Models
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string content { get; set; }
        [ForeignKey("Post")]
        public Guid PostId { get; set; }

        [ForeignKey("User")]
        public Guid UserId {get; set;}
        public string UserName  { get; set; }
    }
}