namespace IdentityService.DTOs
{
    public class AutenticationModel
    {
        public int UserId { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
