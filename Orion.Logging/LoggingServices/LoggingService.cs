using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Logging.LoggingServices
{
    public abstract class LoggingService : IDisposable
    {
		public event Action<Log> OnLog = delegate { };
		public event Action<List<Log>> OnSessionEnd = delegate { };

		private readonly List<Log> _currentSessionLogs = new();

        private bool sessionDisposed = false;

		public List<Log> CurrentSessionLogs
		{
			get => _currentSessionLogs;
		}

		~LoggingService()
		{
            if (!sessionDisposed)
                SessionEnd();

            sessionDisposed = true;
        }
        public void Dispose()
        {
            SessionEnd();
            sessionDisposed = true;
        }

        public abstract List<Log> GetLogsByDateTime(DateTime startDate, DateTime endDate);
        public abstract List<Log> GetLogsByCount(int start, int end);

		public int Log(string message)
		{
            return Log(new Log()
            {
                LogId = Guid.NewGuid(),
                Content = message,
                LogTime = DateTime.Now
            });
        }

        public int Log(string message, DateTime logTime)
        {
            return Log(new Log()
            {
                LogId = Guid.NewGuid(),
                Content = message,
                LogTime = logTime
            });
        }

        public int Log(Log log)
        {
            try
            {
                _currentSessionLogs.Add(log);
                OnLog?.Invoke(log);

                return 1;
            }
            catch
            {
                return 0;
            }

        }

		public int SessionEnd()
        {
            try
            {
                OnSessionEnd?.Invoke(_currentSessionLogs);
                return 1;
            }
            catch
            {
                return 0;
            }
        }
    }
}
