using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Dispatching;
using Orion.Logging.LoggingServices;
using Orion.Server.App.Logging.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Services.Maps;

namespace Orion.Server.App.Logging.ViewModels
{
    public class LogsViewModel : ObservableObject
    {
		private ObservableCollection<Log> _logs;
		public ObservableCollection<Log> Logs
        {
            get => _logs;
            private set { SetProperty(ref _logs, value); }
        }

        private DispatcherQueue _dispatcherQueue;

		public LogsViewModel()
		{
            Logs = new ObservableCollection<Log>();

            _dispatcherQueue = DispatcherQueue.GetForCurrentThread();

            InitialiseLoggingService();
		}

		private void InitialiseLoggingService()
        {
            App.Current.ConfigurationService.GetInstanceOfType<LoggingService>().OnLog += log =>
            {
                _dispatcherQueue.TryEnqueue(() =>
                {
                    Logs.Add(new Log()
                    {
                        LogId = log.LogId,
                        LogTime = log.LogTime,
                        Content = log.Content
                    });
                });
            };

            App.Current.ConfigurationService.GetInstanceOfType<LoggingService>().OnSessionEnd += logs =>
            {
                InitialiseLoggingService();
            };
        }
	}
}
