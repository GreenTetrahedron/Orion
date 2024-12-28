using Orion.Client.Subscriptions;
using Orion.Models.ClientTransmissions;

namespace Orion.Client
{
    public interface IClientService
    {
        public Task<Subscriptable<T>> TransmitDataOfTopic<T>(object? data, string topic) where T : Enum;

        public Task RunClient();

        public Task StopClient();
    }
}
