using GrowDay.Application.Repositories;
using GrowDay.Domain.Entities.Concretes;
using GrowDay.Persistance.DbContexts;
using GrowDay.Persistance.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace GrowDay.Persistence.Repositories;

public class ReadUserTokenRepository : ReadGenericRepository<UserToken>, IReadUserTokenRepository {

    // Constructor

    public ReadUserTokenRepository(GrowDayDbContext context) : base(context) { }

    // Methods

    public async Task<User?> GetUserByRefreshToken(string refreshToken) {
        return await _table.Where(x=> x.Name == "RefreshToken" && !x.IsDeleted && x.Token == refreshToken).Select(p=> p.User).FirstOrDefaultAsync();
    }

    public async Task<User?> GetUserByRePasswordToken(string rePasswordToken) {
        return await _table.Where(x => x.Token == rePasswordToken && x.Name == "RepasswordToken" && !x.IsDeleted).Select(p => p.User).FirstOrDefaultAsync();
    }

    public async Task<User?> GetUserByConfirmEmailToken(string confirmEmailToken) {
        return await _table.Where(x => x.Name == "ConfirmEmailToken" && !x.IsDeleted && x.Token == confirmEmailToken).Select(p => p.User).FirstOrDefaultAsync();
    }

    public async Task<UserToken?> GetConfirmEmailToken(string token) {
        return await _table.Where(x => x.Name == "ConfirmEmailToken" && !x.IsDeleted && x.Token == token).FirstOrDefaultAsync();
    }

    public async Task<UserToken?> GetResetPasswordToken(string token) {
        return await _table.Where(x => x.Name == "RepasswordToken" && !x.IsDeleted && x.Token == token).FirstOrDefaultAsync();
    }
}