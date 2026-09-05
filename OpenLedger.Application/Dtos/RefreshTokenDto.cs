namespace OpenLedger.Application.Dtos
{
    public record RefreshTokenDto(string Token, DateTime ExpiresAt);
}
