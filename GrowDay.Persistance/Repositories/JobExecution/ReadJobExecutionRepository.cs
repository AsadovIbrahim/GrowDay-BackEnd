using GrowDay.Application.Repositories;
using GrowDay.Domain.Entities.Concretes;
using GrowDay.Persistance.DbContexts;
using GrowDay.Persistance.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace GrowDay.Persistance.Repositories
{
    public class ReadJobExecutionRepository : ReadGenericRepository<JobExecutionLog>, IReadJobExecutingRepository
    {
        public ReadJobExecutionRepository(GrowDayDbContext context) : base(context)
        {
        }

        public async Task<JobExecutionLog?> GetByJobName(string jobName)
        {
            return await _table
                .FirstOrDefaultAsync(j => j.JobName == jobName);
        }
    }
}
