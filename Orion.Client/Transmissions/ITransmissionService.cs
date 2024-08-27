using Orion.Client.Subscriptions;
using Orion.Models.ClientTransmissions;

namespace Orion.Client.Transmissions
{
    public interface ITransmissionService
    {
        public Task<Subscriptable<T>> TransmitDataOfTopic<T>(object? data, string topic) where T : Enum;

        public Task<ClientTransmission?> ReceiveData();

        public Task InitialiseRouterConnection();
    }
}
