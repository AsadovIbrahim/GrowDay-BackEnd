using GrowDay.Application.Repositories;
using GrowDay.Domain.Entities.Concretes;
using GrowDay.Persistance.DbContexts;
using GrowDay.Persistance.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace GrowDay.Persistance.Repositories
{
    public class WriteJobExecutionRepository : WriteGenericRepository<JobExecutionLog>, IWriteJobExecutingRepository
    {
        public WriteJobExecutionRepository(GrowDayDbContext context) : base(context)
        {
        }

        public async Task UpdateLastRunDateAsync(string jobName, DateTime lastRunDate)
        {
            var jobExecutionLog = await _table.FirstOrDefaultAsync(j => j.JobName == jobName);

            if (jobExecutionLog != null)
            {
                jobExecutionLog.LastRunDate = lastRunDate;
                _table.Update(jobExecutionLog);
            }
            else
            {
                jobExecutionLog = new JobExecutionLog
                {
                    JobName = jobName,
                    LastRunDate = lastRunDate
                };
                await _table.AddAsync(jobExecutionLog);
            }
            await _context.SaveChangesAsync();
        }

    }
}