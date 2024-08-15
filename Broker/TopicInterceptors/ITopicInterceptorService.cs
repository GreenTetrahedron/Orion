using Orion.Models.ServerTransmissions;
using Orion.Models.ServerTransmissions.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Router.TopicInterceptors
{
    public interface ITopicInterceptorService
    {
        bool TryGetTopicInterceptor(string topic, out Action<ServerResponse> handler);
    }
}
