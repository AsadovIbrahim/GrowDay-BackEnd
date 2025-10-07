using GrowDay.Application.Services;
using GrowDay.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GrowDay.Infrastructure.BackgroundServices
{
    public class NotificationBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<NotificationBackgroundService> _logger;

        public NotificationBackgroundService(IServiceScopeFactory serviceScopeFactory, ILogger<NotificationBackgroundService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Notification Background Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    var userHabitService = scope.ServiceProvider.GetRequiredService<IUserHabitService>();
                    var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

                    var currentTime = DateTime.Now.TimeOfDay;
                    var today = DateTime.Today;
                    var userHabitsResult = await userHabitService.GetAllUserHabitAsync();

                    if (userHabitsResult.Success)
                    {
                        var userHabits = userHabitsResult.Data.Where(h => h.NotificationTime.HasValue);

                        foreach (var habit in userHabits)
                        {
                            // Notification vaxtı uyğun gəlirsə:
                            if (habit.NotificationTime.Value.Hours == currentTime.Hours &&
                                habit.NotificationTime.Value.Minutes == currentTime.Minutes)
                            {
                                // Habit bu gün üçün tamamlanıbsa, notification göndərmə
                                var isCompletedResult = await userHabitService.IsHabitCompletedTodayAsync(habit.UserId, habit.UserHabitId, today);
                                bool isCompleted = isCompletedResult.Success && isCompletedResult.Data;

                                if (!isCompleted)
                                {
                                    _logger.LogInformation($"Sending notification for habit '{habit.Title}' to user '{habit.UserId}'.");
                                    await notificationService.CreateAndSendNotificationAsync(
                                        habit.UserHabitId,
                                        habit.UserId,
                                        habit.Title,
                                        "Time to complete your habit ⏰",
                                        NotificationType.Reminder);
                                }
                                else
                                {
                                    _logger.LogInformation($"Habit '{habit.Title}' for user '{habit.UserId}' is already completed today. Notification not sent.");
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while processing notifications.");
                }
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }

            _logger.LogInformation("Notification Background Service is stopping.");
        }
    }
}