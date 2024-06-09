
using System.ComponentModel.DataAnnotations;


namespace BlogAPI.Dtos
{
    public class LoginDTO
    {   [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
    }
}