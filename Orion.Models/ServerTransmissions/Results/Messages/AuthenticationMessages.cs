using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Orion.Models.ServerTransmissions.Results
{
    public enum AuthenticationMessages
    {
        VALID_CREDENTIALS,
        INVALID_CREDENTIALS,
        USER_ALREADY_SIGNED_IN
    }
}
