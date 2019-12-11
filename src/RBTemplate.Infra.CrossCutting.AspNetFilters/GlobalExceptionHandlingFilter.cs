using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RBTemplate.Domain.Entities.EventLog;
using RBTemplate.Domain.Interfaces;
using RBTemplate.Infra.Data.Repository.EventLogs;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace RBTemplate.Infra.CrossCutting.AspNetFilters
{
    public class GlobalExceptionHandlingFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<GlobalExceptionHandlingFilter> _logger;
        private readonly IHostEnvironment _hostingEnviroment;
        private readonly IEventLogRepository _eventLogRepository;
        private readonly IUser _user;
        public GlobalExceptionHandlingFilter(ILogger<GlobalExceptionHandlingFilter> logger,
            IHostEnvironment hostingEnviroment,
            IEventLogRepository eventLogRepository,
            IUser user)
        {
            _logger = logger;
            _hostingEnviroment = hostingEnviroment;
            _eventLogRepository = eventLogRepository;
            _user = user;
        }

        public override async Task OnExceptionAsync(ExceptionContext context)
        {
            if (_hostingEnviroment.IsProduction())
            {
                _logger.LogError(1, context.Exception, context.Exception.Message);

                var data = new
                {
                    Version = "v1.0",
                    Application = "RBTemplate.IO",
                    Source = "GlobalActionLoggerFilter",
                    User = _user.Name,
                    Hostname = context.HttpContext.Request.Host.Host,
                    Url = context.HttpContext.Request.GetDisplayUrl(),
                    DateTime = DateTime.Now,
                    Method = context.HttpContext.Request.Method,
                    StatusCode = context.HttpContext.Response.StatusCode,
                    Data = context.Exception?.Message,
                    StackTrace = context.Exception?.StackTrace
                };

                var serializedData = JsonSerializer.Serialize(data);

                await _eventLogRepository.AddAsync(
                        new EventLog()
                        {
                            DateLog = DateTime.Now,
                            Data = serializedData
                        });
            }

            base.OnException(context);
        }
    }

}
