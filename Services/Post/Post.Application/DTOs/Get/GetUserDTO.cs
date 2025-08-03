namespace Post.Application.DTOs
{
    public class GetUserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;

        public string Relation { get; set; } = "UNKNOWN"; // Added relation property
    }
}