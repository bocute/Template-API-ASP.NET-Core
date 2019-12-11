using RBTemplate.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RBTemplate.Domain.Notifications
{
    public class DomainNotificationHandler : INotificationHandler
    {
        private List<DomainNotification> _notifications;

        public DomainNotificationHandler()
        {
            _notifications = new List<DomainNotification>();
        }

        public virtual List<DomainNotification> GetNotifications()
        {
            return _notifications;
        }

        public virtual async Task<bool> HasNotificationsAsync()
        {
            return await Task.FromResult(_notifications.Count > 0);
        }

        public Task AddNotificationAsync(DomainNotification notification)
        {
            _notifications.Add(notification);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _notifications = new List<DomainNotification>();
        }
    }
}
