using GrowDay.Application.Repositories.Common;
using GrowDay.Domain.Entities.Concretes;

namespace GrowDay.Application.Repositories
{
    public interface IWriteJobExecutingRepository:IWriteGenericRepository<JobExecutionLog>
    {
        Task UpdateLastRunDateAsync(string jobName, DateTime lastRunDate);
    }
}
