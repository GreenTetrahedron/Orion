using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Server.Users
{
    public class User
    {
        public Guid UserId { get; set; }

        public string Username { get; set; }
    }
}
