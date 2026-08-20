using OpenLedger.Application.Dtos.Auth;
using OpenLedger.Domain.Entities.Auth;

namespace OpenLedger.Application.Interfaces.Repository.Customs
{
    public interface IUserRepository
    {
        public Task<User> CreateUserAsync(RegisterRequestDto request);
        public Task<User> LoginUserAsync(LoginRequestDto request);
    }
}
