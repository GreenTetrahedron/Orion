namespace Orion.Models.ServerTransmissions
{
    public class ServerTransmission
    {
        public ServerResponse Response { get; set; }

        public ServerResponse? Publish { get; set; }

        public ServerTransmission(ServerResponse response, ServerResponse? publish = null)
        {
            Response = response;
            Publish = publish;
        }
    }
}
