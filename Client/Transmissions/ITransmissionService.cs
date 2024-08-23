using Orion.Client.Subscriptables;

namespace Orion.Client.Transmissions
{
    public interface ITransmissionService
    {
        public Task<Subscriptable<T>> TransmitDataOfTopic<T>(object? data, string topic) where T : Enum;
    }
}
