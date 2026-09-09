namespace OpenLedger.Application.Dtos.Auth
{
    public record RefreshTokenDto(string RefreshToken, DateTime RefreshTokenExpires);
}
