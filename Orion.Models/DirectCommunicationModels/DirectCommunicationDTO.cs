using Orion.Models.MessageModels;
using Orion.Models.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Models.DirectCommunicationModels
{
    public class DirectCommunicationDTO
    {
        public Guid DirectCommunicationId { get; set; }

        public List<UserProfile> MemberProfiles { get; set; }

        public List<MessageDTO> Messages { get; set; }
    }
}
