namespace Orion.JsonParser
{
    public interface IJsonService
    {
        public T? DeserialiseJson<T>(string json);

        public string SerialiseObject(object objectToSerialise);
    }
}
