namespace BlogAPI.Dtos
{
    public class UserResponseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string email { get; set; }
        public string Role { get; set; }
    }
}
