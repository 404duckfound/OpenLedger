namespace OpenLedger.Application.Dtos
{
    public record AuthResponseDto(string AccessToken, string RefreshToken, DateTime RefreshTokenExpires);
}
