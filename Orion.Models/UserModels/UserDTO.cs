using Orion.Models.DirectCommunicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Models.UserModels
{
    public class UserDTO
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }

        public List<DirectCommunicationProfile> DirectCommunicationProfiles { get; set; }
    }
}
