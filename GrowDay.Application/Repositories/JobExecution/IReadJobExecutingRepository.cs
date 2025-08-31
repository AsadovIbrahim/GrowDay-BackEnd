using GrowDay.Application.Repositories.Common;
using GrowDay.Domain.Entities.Concretes;

namespace GrowDay.Application.Repositories
{
    public interface IReadJobExecutingRepository:IReadGenericRepository<JobExecutionLog>
    {
        Task<JobExecutionLog?>GetByJobName(string jobName);
    }
}
