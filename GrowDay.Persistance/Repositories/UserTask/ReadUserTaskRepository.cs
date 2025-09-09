using GrowDay.Application.Repositories;
using GrowDay.Domain.Entities.Concretes;
using GrowDay.Persistance.DbContexts;
using GrowDay.Persistance.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace GrowDay.Persistance.Repositories
{
    public class ReadUserTaskRepository : ReadGenericRepository<UserTask>, IReadUserTaskRepository
    {
        public ReadUserTaskRepository(GrowDayDbContext context) : base(context)
        {
        }

        public async Task<int> GetCompletedUserTaskCountByUserIdAsync(string userId)
        {
            var count = await _table.CountAsync(ut => ut.UserId == userId && ut.IsCompleted);
            return count;
        }

        public async Task<IEnumerable<UserTask>> GetCompletedUserTasksByUserIdAsync(string userId)
        {
            return await _table.Where(ut => ut.UserId == userId && ut.IsCompleted).ToListAsync();
        }

        public async Task<UserTask?> GetUserTaskByIdAsync(string userId, string userTaskId)
        {
            return await _table.FirstOrDefaultAsync(ut => ut.Id == userTaskId && ut.UserId == userId);
        }

        public async Task<UserTask?> GetUserTaskByTaskIdAndUserIdAsync(string userId, string taskId)
        {
            return await _table.FirstOrDefaultAsync(ut => ut.UserId == userId && ut.TaskId == taskId);
        }

        public async Task<IEnumerable<UserTask>> GetUserTasksByUserIdAsync(string userId)
        {
            return await _table.Where(ut => ut.UserId == userId).ToListAsync();
        }
    }
}
