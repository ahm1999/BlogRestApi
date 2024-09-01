using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Dtos
{
    public class CreateBlogDTO
    {
        [Required(ErrorMessage = "title is required")]
        public string title { get; set; }
        [Required(ErrorMessage = "Description is required")]
        [MinLength(100)]
        public string Desciption { get; set; }
    }
}
