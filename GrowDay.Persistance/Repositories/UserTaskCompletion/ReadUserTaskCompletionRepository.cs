using GrowDay.Application.Repositories;
using GrowDay.Domain.Entities.Concretes;
using GrowDay.Persistance.DbContexts;
using GrowDay.Persistance.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace GrowDay.Persistance.Repositories
{
    public class ReadUserTaskCompletionRepository : ReadGenericRepository<UserTaskCompletion>, IReadUserTaskCompletionRepository
    {
        public ReadUserTaskCompletionRepository(GrowDayDbContext context) : base(context)
        {
        }

        public async Task<ICollection<UserTaskCompletion>> GetUserTaskCompletions(string userId, string taskId)
        {
            return await _table
                .Where(t=>t.UserTask.UserId==userId && t.UserTaskId==taskId)
                .ToListAsync();
        }
    }
}
