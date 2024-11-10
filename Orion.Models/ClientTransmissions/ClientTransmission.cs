namespace Orion.Models.ClientTransmissions
{
    public class ClientTransmission
    {
        public string Topic { get; set; }

        public object? Data { get; set; }

        public ClientTransmission(string topic, object? data)
        {
            Topic = topic;
            Data = data;
        }
    }
}
