using Orion.Client.Subscriptions;
using Orion.Models.MessageModels;
using Orion.Models.ServerTransmissions.Results.Messages;

namespace Orion.Client.Messages.Services
{
    public interface IMessageService
    {
        public Task<Subscriptable<NewMessageMessages>> SendDirectMessage(NewDirectMessage newDirectMessage);
        public Task<Subscriptable<NewMessageMessages>> SendGroupMessage(NewGroupMessage newGroupMessage);
    }
}
