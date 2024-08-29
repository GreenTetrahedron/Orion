using Orion.Models.DirectCommunicationModels;
using Orion.Models.UserModels;
using Orion.Server.Messages;
using Orion.Server.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Server.DirectCommuncations
{
    public class DirectCommunication
    {
        public Guid DirectCommunicationId { get; set; }

        public List<User> Members { get; set; }

        public List<Message> Messages { get; set; }

        public static explicit operator DirectCommunicationDTO(DirectCommunication directCommunication) =>
            new DirectCommunicationDTO()
            {
                DirectCommunicationId = directCommunication.DirectCommunicationId,
                MemberProfiles = directCommunication.Members.Select(x => (UserProfile)x).ToList()
            };

        public static implicit operator DirectCommunicationProfile(DirectCommunication directCommunication) =>
            new DirectCommunicationProfile
            {
                DirectCommunicationId = directCommunication.DirectCommunicationId,
                MemberProfiles = directCommunication.Members.Select(member => (UserProfile)member).ToList()
            };
    }
}
