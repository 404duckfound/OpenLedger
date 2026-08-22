namespace OpenLedger.Application.Dtos.Auth
{
    public class AuthResponseDto
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
        public required DateTime RefreshTokenExpires { get; set; }
    }
}
