namespace OpenLedger.Application.Dtos.Auth
{
    public record RefreshTokenDto(string Token, DateTime ExpiresAt);
}
