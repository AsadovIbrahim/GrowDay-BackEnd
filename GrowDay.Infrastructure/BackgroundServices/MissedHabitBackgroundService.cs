using GrowDay.Application.Repositories;
using GrowDay.Domain.Entities.Concretes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GrowDay.Infrastructure.BackgroundServices
{
    public class MissedHabitBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<MissedHabitBackgroundService> _logger;

        private const string JobName = "MissedHabitJob";

        public MissedHabitBackgroundService(IServiceProvider serviceProvider, ILogger<MissedHabitBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Missed Habit Background Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var readJobRepo = scope.ServiceProvider.GetRequiredService<IReadJobExecutingRepository>();
                    var writeJobRepo = scope.ServiceProvider.GetRequiredService<IWriteJobExecutingRepository>();

                    var log = await readJobRepo.GetByJobName(JobName);

                    // İlk dəfə işdirsə və ya son işləmə tarixi varsa
                    var lastRunDate = log?.LastRunDate.Date ?? DateTime.UtcNow.Date.AddDays(-7);
                    var today = DateTime.UtcNow.Date;

                    int totalMissed = 0;

                    // Hər gün üçün missed habitləri hesabla
                    for (var day = lastRunDate; day <= today; day = day.AddDays(1))
                    {
                        totalMissed += await RunMissedHabitsLogicAsync(day, stoppingToken);
                    }

                    // Son işləmə tarixini DB-də yenilə
                    if (log == null)
                    {
                        await writeJobRepo.AddAsync(new JobExecutionLog
                        {
                            JobName = JobName,
                            LastRunDate = today
                        });
                    }
                    else
                    {
                        await writeJobRepo.UpdateLastRunDateAsync(JobName, today);
                    }

                    _logger.LogInformation("Missed habits job executed up to {date} UTC. Total missed: {count}", today, totalMissed);

                    // Növbəti run vaxtını təyin et (sabah 00:10 UTC)
                    var nextRun = today.AddDays(1).AddMinutes(10);
                    var delay = nextRun - DateTime.UtcNow;
                    if (delay.TotalMilliseconds > 0)
                        await Task.Delay(delay, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while executing missed habits background job.");
                }
            }
        }

        private async Task<int> RunMissedHabitsLogicAsync(DateTime day, CancellationToken stoppingToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var readUserHabitRepository = scope.ServiceProvider.GetRequiredService<IReadUserHabitRepository>();
            var readHabitRecordRepository = scope.ServiceProvider.GetRequiredService<IReadHabitRecordRepository>();
            var writeHabitRecordRepository = scope.ServiceProvider.GetRequiredService<IWriteHabitRecordRepository>();

            var userHabits = await readUserHabitRepository.GetAllActiveUserHabitsAsync();

            int missedCount = 0;

            foreach (var userHabit in userHabits!)
            {
                // Düzəliş: yalnız start date-dən sonrakı günlər üçün missed habit yaradılır
                if (!userHabit.StartDate.HasValue || userHabit.StartDate.Value > day)
                    continue;

                var habitRecord = await readHabitRecordRepository.GetByUserHabitIdAndDateAsync(userHabit.Id, day);
                if (habitRecord == null)
                {
                    var missedHabitRecord = new HabitRecord
                    {
                        UserHabitId = userHabit.Id,
                        Date = day,
                        IsCompleted = false,
                        Note = null,
                        CreatedAt = DateTime.UtcNow
                    };
                    await writeHabitRecordRepository.AddAsync(missedHabitRecord);
                    missedCount++;
                }
            }

            _logger.LogInformation("Missed habits job executed for {date} UTC. Missed: {count}", day, missedCount);

            return missedCount;
        }
    }
}