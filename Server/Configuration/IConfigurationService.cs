using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Server.Configuration
{
    public interface IConfigurationService
    {
        T GetInstanceOfType<T>();

        object GetInstanceOfType(Type type);

        void AddInstanceOfType<T>(T instance);
    }
}
