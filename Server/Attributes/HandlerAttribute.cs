namespace Orion.Server.Attributes
{
    public class HandlerAttribute : Attribute
    {
        public string Topic { get; set; }

        public HandlerAttribute(string topic)
        {
            Topic = topic;
        }
    }
}
