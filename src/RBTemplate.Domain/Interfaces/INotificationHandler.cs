using RBTemplate.Domain.Notifications;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RBTemplate.Domain.Interfaces
{
    public interface INotificationHandler
    {
        public abstract Task AddNotificationAsync(DomainNotification notification);
        public abstract List<DomainNotification> GetNotifications();
        public abstract Task<bool> HasNotificationsAsync();
    }
}
