using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Logging
{
    public class Log
    {
        public Guid LogId { get; set; }

        public DateTime LogTime { get; set; }

        public string Content { get; set; }
    }
}
