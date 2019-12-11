using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RBTemplate.Domain.Entities.EventLog;

namespace RBTemplate.Infra.Data.Repository.EventLogs
{
    public interface IEventLogRepository : IDisposable
    {
        Task AddAsync(EventLog log);
    }
}
