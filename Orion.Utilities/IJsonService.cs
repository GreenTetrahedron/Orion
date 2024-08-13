using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.JsonParser
{
    public interface IJsonService
    {
        public T? DeserialiseJson<T>(string json);

        public string SerialiseObject(object objectToSerialise);
    }
}
