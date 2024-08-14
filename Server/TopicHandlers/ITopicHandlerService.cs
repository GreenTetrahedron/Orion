using Orion.Models.ServerTransmissions.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Server.TopicHandlers
{
    public interface ITopicHandlerService
    {
        Func<object, Task<ServerResult>>? GetTopicHandler(string topic);
    }
}
