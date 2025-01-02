using Orion.Server.Messages;
using Orion.Server.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Server.Groups
{
    public class Group
    {
        public Guid GroupId { get; set; }

        public string GroupName { get; set; }

        public List<User> Members { get; set; }

        public List<Message> Messages { get; set; }
    }
}
