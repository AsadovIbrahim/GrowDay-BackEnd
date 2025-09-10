using GrowDay.Application.Repositories.Common;
using GrowDay.Domain.Entities.Concretes;

namespace GrowDay.Application.Repositories
{
    public interface IReadUserTaskCompletionRepository:IReadGenericRepository<UserTaskCompletion>
    {
        Task<ICollection<UserTaskCompletion>> GetUserTaskCompletions(string userId, string taskId);

    }
}
