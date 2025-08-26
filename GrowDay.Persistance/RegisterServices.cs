using GrowDay.Application.Repositories;
using GrowDay.Application.Services;
using GrowDay.Persistance.DbContexts;
using GrowDay.Persistance.Repositories;
using GrowDay.Persistance.Services;
using GrowDay.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GrowDay.Persistance
{
    public static class RegisterServices
    {
        public static void AddPersistenceRegister(this IServiceCollection services)
        {
            //Services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IHabitService, HabitService>();
            services.AddScoped<IUserHabitService, UserHabitService>();
            services.AddScoped<IHabitRecordService, HabitRecordService>();
            services.AddScoped<IStatisticService, StatisticService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IUserPreferencesService, UserPreferencesService>();
            services.AddScoped<ISuggestedHabitService, SuggestedHabitService>();

            // Context

            services.AddDbContext<GrowDayDbContext>(options =>
            {
                ConfigurationBuilder configurationBuilder = new();
                var builder = configurationBuilder.AddJsonFile("appsettings.json").Build();

                options.UseLazyLoadingProxies()
                    .UseSqlServer(builder.GetConnectionString("Default"));
            });

            // All Write Repositories
            services.AddScoped<IWriteUserRepository, WriteUserRepository>();
            services.AddScoped<IWriteUserTokenRepository, WriteUserTokenRepository>();
            services.AddScoped<IWriteHabitRepository, WriteHabitRepository>();
            services.AddScoped<IWriteUserHabitRepository, WriteUserHabitRepository>();
            services.AddScoped<IWriteHabitRecordRepository, WriteHabitRecordRepository>();
            services.AddScoped<IWriteStatisticRepository, WriteStatisticRepository>();
            services.AddScoped<IWriteNotificationRepository, WriteNotificationRepository>();
            services.AddScoped<IWriteUserPreferencesRepository, WriteUserPreferencesRepository>();
            services.AddScoped<IWriteSuggestedHabitRepository, WriteSuggestedHabitRepository>();



            // All Read Repositories
            services.AddScoped<IReadUserRepository, ReadUserRepository>();
            services.AddScoped<IReadUserTokenRepository, ReadUserTokenRepository>();
            services.AddScoped<IReadHabitRepository, ReadHabitRepository>();
            services.AddScoped<IReadUserHabitRepository, ReadUserHabitRepository>();
            services.AddScoped<IReadHabitRecordRepository, ReadHabitRecordRepository>();
            services.AddScoped<IReadStatisticRepository, ReadStatisticRepository>();
            services.AddScoped<IReadNotificationRepository, ReadNotificationRepository>();
            services.AddScoped<IReadUserPreferencesRepository, ReadUserPreferencesRepository>();
            services.AddScoped<IReadSuggestedHabitRepository, ReadSuggestedHabitRepository>();
        }
    }
}
