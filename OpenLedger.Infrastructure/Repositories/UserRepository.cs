using Microsoft.EntityFrameworkCore;
using OpenLedger.Application.Dtos.Auth;
using OpenLedger.Application.Interfaces.Repository.Customs;
using OpenLedger.Domain.Entities.Auth;
using OpenLedger.Infrastructure.Contexts;

namespace OpenLedger.Infrastructure.Repositories
{
    public class UserRepository(AppDbContext context) : IUserRepository
    {
        public async Task<User> CreateUserAsync(RegisterRequestDto request)
        {
            if (await context.Users.FirstOrDefaultAsync(u => u.Email == request.Email) != null)
            {
                throw new InvalidOperationException("User with this email already exists.");
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User
            (
                request.Name,
                request.Email,
                passwordHash
            );

            context.Users.Add(user);
            await context.SaveChangesAsync();

            return await Task.FromResult(user);
        }
        public async Task<User> LoginUserAsync(LoginRequestDto request)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == request.Email) ?? throw new InvalidOperationException("Invalid email or password.");

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                throw new InvalidOperationException("Invalid email or password.");
            }

            return await Task.FromResult(user);
        }
    }
}
