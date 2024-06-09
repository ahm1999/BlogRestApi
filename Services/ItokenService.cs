

using BlogAPI.Models;

namespace BlogAPI.Services
{
    public interface ItokenService
    {
        string CreateToken (User user);
    }
}