namespace cmkapi.DTO
{
    public class LoginResponse
    {
        public string AccessToken { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;

        public DateTime ExpiredAt { get; set; }

        public UserResponse? User { get; set; } = null;
    }
}