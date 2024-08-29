using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Models.UserModels
{
    public class UserProfile
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
    }
}
