
using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Dtos
{
    public class PostDto_Req
    {   
        [Required(ErrorMessage = "title is required")]
        public String Title { get; set; } 
        [Required(ErrorMessage = "content is required")]
        public String Content { get; set; } 

    }
}