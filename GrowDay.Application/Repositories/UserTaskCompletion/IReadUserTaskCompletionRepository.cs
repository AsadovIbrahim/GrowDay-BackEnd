using GrowDay.Application.Repositories.Common;
using GrowDay.Domain.Entities.Concretes;

namespace GrowDay.Application.Repositories
{
    public interface IReadUserTaskCompletionRepository:IReadGenericRepository<UserTaskCompletion>
    {
        Task<IEnumerable<UserTaskCompletion>> GetUserCompletionsByUserIdAsync(string userId);
        Task<ICollection<UserTaskCompletion>> GetUserTaskCompletions(string userId, string taskId);
    }
}
