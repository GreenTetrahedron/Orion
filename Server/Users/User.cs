using Orion.Models.DirectCommunicationModels;
using Orion.Models.UserModels;
using Orion.Server.DirectCommuncations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Server.Users
{
    public class User
    {
        public Guid UserId { get; set; }

        public string Username { get; set; }

        public List<DirectCommunication> DirectCommunications { get; set; }

        public static implicit operator UserProfile(User user) =>
            new UserProfile { UserId = user.UserId, Username = user.Username };

        public static explicit operator UserDTO(User user) =>
            new UserDTO
            {
                UserId = user.UserId,
                Username = user.Username,
                DirectCommunicationProfiles = user.DirectCommunications.Select(directCommunication => (DirectCommunicationProfile)directCommunication).ToList()
            };
    }
}
