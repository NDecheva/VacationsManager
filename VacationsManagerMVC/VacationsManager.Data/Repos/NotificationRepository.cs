using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationsManager.Data.Entities;
using VacationsManager.Shared.Attributes;
using VacationsManager.Shared.Dtos;
using VacationsManager.Shared.Repos.Contracts;
using YourNamespace.Data.Repos;

namespace VacationsManager.Data.Repos
{
    [AutoBind]
    public class NotificationRepository : BaseRepository<Notification, NotificationDto>, INotificationRepository
    {
        public NotificationRepository(VacationsManagerDbContext context, IMapper mapper) : base(context, mapper) { }

        public async Task<IEnumerable<NotificationDto>> GetUnreadNotificationsAsync(int recipientId)
        {
            var unreadNotifications = await _context.Set<Notification>()
                .Where(n => n.RecipientId == recipientId && !n.IsRead)
                .ToListAsync();

            return MapToEnumerableOfModel(unreadNotifications);
        }

    }
}
