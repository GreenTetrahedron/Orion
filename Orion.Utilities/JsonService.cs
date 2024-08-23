using Newtonsoft.Json;
using System.Runtime.Serialization.Formatters;

namespace Orion.JsonParser
{
    public class JsonService : IJsonService
    {
        private readonly JsonSerializerSettings _jsonSerialiserSettings;

        public JsonService()
        {
            _jsonSerialiserSettings = new()
            {
                TypeNameHandling = TypeNameHandling.All
            };
        }

        public T? DeserialiseJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, _jsonSerialiserSettings);
        }

        public string SerialiseObject(object objectToSerialise)
        {
            return JsonConvert.SerializeObject(objectToSerialise, Formatting.None, _jsonSerialiserSettings);
        }
    }
}
