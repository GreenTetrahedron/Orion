using Newtonsoft.Json;

namespace Orion.JsonParser
{
    public class JsonService : IJsonService
    {
        public T? DeserialiseJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public string SerialiseObject(object objectToSerialise)
        {
            return JsonConvert.SerializeObject(objectToSerialise);
        }
    }
}
