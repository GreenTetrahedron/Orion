using Orion.Logging.DataLayer;
using System;
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

        private void SaveLogToDB(Log log)
        {
            _context.Logs.Add(log);
            _context.SaveChanges();
        }
    }
}
