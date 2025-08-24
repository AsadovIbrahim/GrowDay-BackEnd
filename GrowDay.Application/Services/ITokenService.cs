using GrowDay.Domain.Entities.Concretes;
using GrowDay.Domain.Helpers;

namespace GrowDay.Application.Services
{
    public interface ITokenService
    {
        TokenCredentials CreateRefreshToken();
        TokenCredentials CreateConfirmEmailToken();
        Task<TokenCredentials> CreateAccessToken(User user);
        Task<TokenCredentials> CreateRepasswordToken(User user);
    }
}
