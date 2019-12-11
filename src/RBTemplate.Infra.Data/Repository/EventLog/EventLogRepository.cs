using RBTemplate.Domain.Entities.EventLog;
using RBTemplate.Infra.Data.Contexts;
using System.Threading.Tasks;

namespace RBTemplate.Infra.Data.Repository.EventLogs
{
    public class EventLogRepository : IEventLogRepository
    {
        private readonly LogContext _context;
        public EventLogRepository(LogContext context)
        {
            _context = context;
        }
        public async Task AddAsync(EventLog log)
        {
            _context.EventLog.Add(log);
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
