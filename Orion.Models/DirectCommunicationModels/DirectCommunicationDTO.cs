using Orion.Models.MessageModels;
using Orion.Models.UserModels;

namespace Orion.Models.DirectCommunicationModels
{
    public class DirectCommunicationDTO
    {
        public Guid DirectCommunicationId { get; set; }

        public List<UserProfile> MemberProfiles { get; set; }

        public List<MessageDTO> Messages { get; set; }
    }
}
