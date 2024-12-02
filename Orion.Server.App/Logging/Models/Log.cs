using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Server.App.Logging.Models
{
    public partial class Log : ObservableObject
    {
        [ObservableProperty]
        private Guid logId;

        [ObservableProperty]
        private string content;

        [ObservableProperty]
        private DateTime logTime;
    }
}
