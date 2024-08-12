using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.JsonParser
{
    public static class JsonParserService
    {
        public static T? DeserialiseJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
        public static string SerialiseObject(object objectToSerialise)
        {
            return JsonConvert.SerializeObject(objectToSerialise);
        }
    }
}
