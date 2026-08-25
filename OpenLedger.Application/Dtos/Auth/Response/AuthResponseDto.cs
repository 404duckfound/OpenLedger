namespace OpenLedger.Application.Dtos.Auth.Response
{
    public record AuthResponseDto(string AccessToken, string RefreshToken, DateTime RefreshTokenExpires) {}
}
