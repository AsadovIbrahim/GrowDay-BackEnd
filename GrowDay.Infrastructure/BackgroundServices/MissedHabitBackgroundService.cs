using GrowDay.Application.Repositories;
using GrowDay.Domain.Entities.Concretes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GrowDay.Infrastructure.BackgroundServices
{
    public class MissedHabitBackgroundService:BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<MissedHabitBackgroundService> _logger;

        public MissedHabitBackgroundService(IServiceProvider serviceProvider, ILogger<MissedHabitBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested) {
                try
                {
                    var now = DateTime.UtcNow;
                    var nextRun = now.Date.AddDays(1).AddMinutes(10);
                    var delay = nextRun - now;

                    if (delay.TotalMilliseconds > 0)
                        await Task.Delay(delay, stoppingToken);

                    using var scope = _serviceProvider.CreateScope();
                    var readUserHabitRepository = scope.ServiceProvider.GetRequiredService<IReadUserHabitRepository>();
                    var readHabitRecordRepository = scope.ServiceProvider.GetRequiredService<IReadHabitRecordRepository>();
                    var writeHabitRecordRepository = scope.ServiceProvider.GetRequiredService<IWriteHabitRecordRepository>();

                    var today = DateTime.UtcNow.Date;
                    var userHabits = await readUserHabitRepository.GetAllActiveUserHabitsAsync();

                    foreach (var userHabit in userHabits!)
                    {
                     
                        var habitRecord = await readHabitRecordRepository.GetByUserHabitIdAndDateAsync(userHabit.Id, today);
                        if (habitRecord == null)
                        {
                            var missedHabitRecord = new HabitRecord
                            {
                                UserHabitId = userHabit.Id,
                                Date = today,
                                IsCompleted = false,
                                Note = null,
                                CreatedAt = DateTime.UtcNow
                            };
                            await writeHabitRecordRepository.AddAsync(missedHabitRecord);
                        }
                    }
                    _logger.LogInformation("Missed habits job executed for {date}.", today);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while executing missed habits background job.");
                }
            }
        }
    }
}
