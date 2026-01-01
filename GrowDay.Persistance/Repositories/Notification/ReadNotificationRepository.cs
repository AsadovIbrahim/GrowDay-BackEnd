using GrowDay.Persistance.DbContexts;
using GrowDay.Application.Repositories;
using GrowDay.Domain.Entities.Concretes;
using GrowDay.Persistance.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace GrowDay.Persistance.Repositories
{
    public class ReadNotificationRepository : ReadGenericRepository<Notification>, IReadNotificationRepository
    {
        public ReadNotificationRepository(GrowDayDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(string userId, int pageIndex = 0, int pageSize = 10)
        {
            return await _table
                .Where(n => n.UserId == userId)
                .OrderBy(n => n.IsRead)
                    .ThenByDescending(n => n.CreatedAt)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
