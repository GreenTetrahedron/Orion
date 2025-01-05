using Orion.JsonParser;
using Orion.Router.Models;
using Orion.Transport.ConnectionServices;
using Orion.Transport.TransmissionServices;
using System.Collections.Concurrent;
using System.Net.Sockets;

namespace Orion.Router.Clients
{
    public class ClientService : IClientService
    {
        private readonly IJsonService _jsonService;

        private readonly ConcurrentDictionary<Guid, LifeSupport> _userIdToLifeSupport;

        public ClientService(IJsonService jsonService)
        {
            _jsonService = jsonService;
            _userIdToLifeSupport = new();
        }

        public async Task<LifeSupport> InstantiateConnection(Socket socket)
        {
            var lifeSupport = new LifeSupport(socket, _jsonService);

            await lifeSupport.InitialiseConnection();
            
            lifeSupport.OnLogIn += userId => _userIdToLifeSupport.TryAdd(userId, lifeSupport);

            return lifeSupport;
        }

        public bool TerminateConnection(ref LifeSupport connection)
        {
            return _userIdToLifeSupport.TryRemove(connection.UserId, out _);
        }

        public bool TryGetClientConnection(Guid userId, out LifeSupport connection)
        {
            return _userIdToLifeSupport.TryGetValue(userId, out connection);
        }
    }
}
