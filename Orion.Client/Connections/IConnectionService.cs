using Orion.Client.Transmissions;

namespace Orion.Client.Connections
{
    public interface IConnectionService
    {
        public Task<bool> SendMessage(byte[] data);
        public Task<MessageBytes> ReceiveMessage();
    }
}
