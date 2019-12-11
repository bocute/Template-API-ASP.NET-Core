using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RBTemplate.Domain.Business.Example;
using RBTemplate.Domain.Business.Example.Repository;
using RBTemplate.Domain.Entities.Exemplo.Business;
using RBTemplate.Domain.Interfaces;
using RBTemplate.Domain.Notifications;
using RBTemplate.Infra.CrossCutting.AspNetFilters;
using RBTemplate.Infra.CrossCutting.Identity.Data;
using RBTemplate.Infra.CrossCutting.Identity.Models;
using RBTemplate.Infra.CrossCutting.Identity.SendEmail;
using RBTemplate.Infra.Data.Contexts;
using RBTemplate.Infra.Data.Repository;
using RBTemplate.Infra.Data.Repository.EventLogs;
using RBTemplate.Infra.Data.UoW;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace RBTemplate.Infra.CrossCutting.IoC
{
    public class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            // ASPNET
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Domain
            services.AddScoped<INotificationHandler, DomainNotificationHandler>();
            services.AddScoped<IExampleBusiness, ExampleBusiness>();

            // Infra - Data
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ExampleContext>();


            // Infra - Contexto das classes
            services.AddScoped<IExampleRepository, ExampleRepository>();

            // Infra - Identity
            services.AddScoped<DbContext, ApplicationDbContext>();
            services.AddScoped<IUser, AspNetUser>();

            //Configurações E-mail
            services.AddTransient<IEmailSender, EmailSender>();

            // Infra - Logs
            services.AddScoped<LogContext>();
            services.AddScoped<IEventLogRepository, EventLogRepository>();


            // Infra - Filtros
            services.AddScoped<ILogger<GlobalExceptionHandlingFilter>, Logger<GlobalExceptionHandlingFilter>>();
            services.AddScoped<GlobalExceptionHandlingFilter>();

            services.AddScoped<HttpClient>();
        }
    }
}
