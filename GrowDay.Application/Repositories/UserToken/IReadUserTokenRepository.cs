using GrowDay.Application.Repositories.Common;
using GrowDay.Domain.Entities.Concretes;

namespace GrowDay.Application.Repositories;

public interface IReadUserTokenRepository : IReadGenericRepository<UserToken>{

    // Methods

    Task<UserToken?> GetConfirmEmailToken(string token);
    Task<UserToken?> GetResetPasswordToken(string token);
    Task<User?> GetUserByRefreshToken(string refreshToken);
    Task<User?> GetUserByRePasswordToken(string rePasswordToken);
    Task<User?> GetUserByConfirmEmailToken(string confirmEmailToken);
}
