using GrowDay.Domain.Entities.Concretes;
using GrowDay.Application.Repositories.Common;

namespace GrowDay.Application.Repositories
{
    public interface IReadUserPreferencesRepository : IReadGenericRepository<UserPreferences>
    {
        Task<UserPreferences?> GetPreferencesByUserIdAsync(string userId);
    }
}
