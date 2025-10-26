using GrowDay.Application.Repositories;
using GrowDay.Domain.Entities.Concretes;
using GrowDay.Persistance.DbContexts;
using GrowDay.Persistance.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace GrowDay.Persistance.Repositories
{
    public class ReadUserActivityRepository : ReadGenericRepository<UserActivity>, IReadUserActivityRepository
    {
        public ReadUserActivityRepository(GrowDayDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<UserActivity>> GetUserActivitiesAsync(string userId, int pageIndex = 0, int pageSize = 10)
        {
            return await _table
                .Where(ua => ua.UserId == userId && !ua.IsDeleted)
                .OrderByDescending(x => x.CreatedAt)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
