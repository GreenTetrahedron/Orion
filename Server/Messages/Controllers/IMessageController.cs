using Orion.Models.MessageModels;
using Orion.Models.ServerTransmissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Server.Messages.Controllers
{
    public interface IMessageController
    {
        public Task<ServerTransmission> AddDirectMessage(NewDirectMessage newDirectMessage);
    }
}
