using Orion.Transport.ConnectionServices;

namespace Orion.Transport.ConnectionServices
{
    public interface IConnectionService
    {
        public Task<int> SendMessage(byte[] data);

        public Task<MessageBytes> ReceiveMessage();
    }
}
