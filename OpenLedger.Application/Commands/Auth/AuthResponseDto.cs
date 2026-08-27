namespace OpenLedger.Application.Commands.Auth
{
    public record AuthResponseDto(string AccessToken, string RefreshToken, DateTime RefreshTokenExpires);
}
