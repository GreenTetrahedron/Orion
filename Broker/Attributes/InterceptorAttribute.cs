namespace Orion.Router.Attributes
{
    public class InterceptorAttribute : Attribute
    {
        public string Topic { get; set; }

        public InterceptorAttribute(string topic)
        {
            Topic = topic;
        }
    }
}
