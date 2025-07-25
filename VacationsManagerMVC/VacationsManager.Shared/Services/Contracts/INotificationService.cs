﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationsManager.Shared.Dtos;
using VacationsManager.Shared.Repos.Contracts;

namespace VacationsManager.Shared.Services.Contracts
{
    public interface INotificationService : IBaseCrudService<NotificationDto, INotificationRepository>
    {
        public Task SendNotificationAsync(int recipientId, string message);
        Task MarkAsReadAsync(int notificationId);
        Task<IEnumerable<NotificationDto>> GetUnreadNotificationsAsync(int userId);
    }
}
