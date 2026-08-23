namespace OpenLedger.Application.Dtos.Auth
{
    public record AuthResponseDto(string AccessToken, string RefreshToken, DateTime RefreshTokenExpires) {}
}
