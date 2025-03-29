using IdentityServer.Entities;
using IdentityService.DTOs;

namespace IdentityService.Services
{
    public interface IAuthentiactionService
    {
        Task<User> ValidateUser(UserCredentialsDto userCredentials);
        Task<AutenticationModel> CreateAutenticationModel(User user);
        Task RemoveRefreshToken(User user, string refreshToken);
    }
}
