namespace IdentityService.DTOs
{
    public class UserRelationDTO
    {
        public int Id { get; set; }
        public string Username { get; set; } =string.Empty;
        public string Relation { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

    }
}