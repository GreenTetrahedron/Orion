using Orion.Models.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Server.Attributes
{
    public class AuthoriseAttribute : Attribute
    {
        public Roles Role { get; set; }

        public AuthoriseAttribute(Roles role)
        {
            Role = role;
        }
    }
}
