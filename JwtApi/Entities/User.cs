namespace JwtApi.Entities
{
    public class User
    {
        public int UserId { get; set; } 
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;      

    }
}
