using Orion.Client.Subscriptions;
using Orion.Models.MessageModels;
using Orion.Models.ServerTransmissions.Results.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Client.Messages.Services
{
    public interface IMessageService
    {
        public Task<Subscriptable<NewMessageMessages>> SendDirectMessage(NewDirectMessage newDirectMessage);
    }
}
