﻿using GrowDay.Domain.Entities.Concretes;
using Microsoft.AspNetCore.SignalR;

namespace GrowDay.Infrastructure.Hubs
{
    public class NotificationHub:Hub
    {
        public async Task SendNotification(string userHabitId,string userId, string title, string message)
        {
            await Clients.User(userId).SendAsync("ReceiveNotification", new
            {
                UserHabitId = userHabitId,
                Title = title,
                Message = message
            });
        }
    }
}
