using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.JsonParser
{
    public class JsonParserService : IJsonParserService
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
