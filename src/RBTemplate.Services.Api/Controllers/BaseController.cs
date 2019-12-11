using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RBTemplate.Domain.Interfaces;
using RBTemplate.Domain.Notifications;

namespace RBTemplate.Services.Api.Controllers
{
    [Produces("application/json")]
    public abstract class BaseController : ControllerBase
    {
        protected Guid UserId { get; set; }

        private readonly INotificationHandler _notifications;
        
        protected BaseController(INotificationHandler notifications, IUser user)
        {
            _notifications = notifications;

            if (user.IsAuthenticated())
            {
                UserId = user.GetUserId();
            }
        }

        protected async Task<IActionResult> ResponseAsync(object result = null)
        {
            if (await IsValidAsync())
            {
                return Ok(new
                {
                    success = true,
                    data = result
                });
            }

            return BadRequest(new
            {
                success = false,
                errors = _notifications.GetNotifications().Select(n => n.Value)
            });
        }

        protected async Task<bool> IsValidAsync()
        {
            return (!await _notifications.HasNotificationsAsync());
        }

        protected async Task NotifyErrorModelInvalid()
        {
            var errors = ModelState.Values.SelectMany(m => m.Errors);

            foreach (var error in errors)
            {
                var errorMessage = error.Exception == null ? error.ErrorMessage : error.Exception.Message;
                await NotifyError(string.Empty, errorMessage);
            }
        }

        protected async Task NotifyError(string key, string value)
        {
            await _notifications.AddNotificationAsync(new DomainNotification(key, value));
        }

        protected async Task<bool> ValidModelState()
        {
            if (ModelState.IsValid) return true;

            await NotifyErrorModelInvalid();
            return false;
        }
    }
}