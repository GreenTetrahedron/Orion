using Orion.Logging.DataLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Logging.LoggingServices
{
    public class DbLoggingService : LoggingService
    {
        private readonly LoggingDbContext _context;

        public DbLoggingService(LoggingDbContext context) : base()
        {
            _context = context;

            OnLog += SaveLogToDB;
        }

        public override List<Log> GetLogsByCount(int start, int end)
        {
            var logs = _context.Logs
                .OrderByDescending(log => log.LogTime)
                .Take(end)
                .TakeLast(end - start)
                .ToList();

            return logs;
        }

        public override List<Log> GetLogsByDateTime(DateTime startDate, DateTime endDate)
        {
            var logs = _context.Logs
                .Where(log => log.LogTime <= endDate && log.LogTime >= startDate)
                .OrderByDescending(log => log.LogTime)
                .ToList();

            return logs;
        }

        private void SaveLogToDB(Log log)
        {
            _context.Logs.Add(log);
            _context.SaveChanges();
        }
    }
}
