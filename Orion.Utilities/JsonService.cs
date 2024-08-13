using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
