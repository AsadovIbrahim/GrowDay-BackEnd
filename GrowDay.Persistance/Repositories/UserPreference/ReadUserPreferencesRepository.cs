using GrowDay.Application.Repositories;
using GrowDay.Domain.Entities.Concretes;
using GrowDay.Persistance.DbContexts;
using GrowDay.Persistance.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace GrowDay.Persistance.Repositories
{
    public class ReadUserPreferencesRepository : ReadGenericRepository<UserPreferences>, IReadUserPreferencesRepository
    {
        public ReadUserPreferencesRepository(GrowDayDbContext context) : base(context)
        {
        }

        public async Task<UserPreferences?> GetPreferencesByUserIdAsync(string userId)
        {
            return await _table.FirstOrDefaultAsync(up => up.UserId == userId);
        }
    }
}
