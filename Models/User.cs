
namespace BlogAPI.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string email { get; set; }
        public string password { get; set; }

        public string Role  {get ; set ;}

        public ICollection <Post> posts {get ;set;}
    }

}