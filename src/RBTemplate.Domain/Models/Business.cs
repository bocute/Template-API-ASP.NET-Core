using FluentValidation.Results;
using RBTemplate.Domain.Interfaces;
using RBTemplate.Domain.Notifications;
using System;
using System.Threading.Tasks;

namespace RBTemplate.Domain.Models
{
    public abstract class Business<T> where T : Entity<T>
    {
        private readonly IUnitOfWork _uow;
        private readonly INotificationHandler _notifications;
        protected abstract Task<bool> ExistsAsync(Guid? id, string usuarioId, string messageType);

        public Business(IUnitOfWork uow, INotificationHandler notifications)
        {
            _uow = uow;
            _notifications = notifications;
        }

        protected async Task<bool> Commit()
        {
            if (await _notifications.HasNotificationsAsync()) return false;
            if (await _uow.Commit()) return true;

            await _notifications.AddNotificationAsync(new DomainNotification("Commit", "Ocorreu um erro ao salvar os dados no banco"));
            return false;
        }

        protected virtual async Task<bool> IsValidAsync(Entity<T> obj)
        {
            if (obj.IsValid()) return true;

            await NotifyValidationErrorAsync(obj.ValidationResult);

            return false;
        }

        protected async Task NotifyValidationErrorAsync(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                await _notifications.AddNotificationAsync(new DomainNotification(error.PropertyName, error.ErrorMessage));
            }
        }
    }
}
