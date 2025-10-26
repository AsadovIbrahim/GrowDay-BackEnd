using GrowDay.Domain.Entities.Concretes;
using GrowDay.Application.Repositories.Common;

namespace GrowDay.Application.Repositories
{
    public interface IReadAchievementRepository : IReadGenericRepository<Achievement>
    {
        Task<IEnumerable<Achievement>> GetAllAchievementsAsync(int pageIndex = 0, int pageSize = 10);
    }
}
