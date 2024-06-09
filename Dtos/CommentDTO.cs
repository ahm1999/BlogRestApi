using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Dtos
{
    public class CommentDTO
    {
    [Required]
    public string content { get; set; }

    }
}