using Orion.Models.DirectCommunicationModels;
using Orion.Models.GroupModels;

namespace Orion.Models.UserModels
{
    public class UserDTO
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }

        public List<DirectCommunicationProfile> DirectCommunicationProfiles { get; set; }
        public List<GroupProfile> GroupProfiles { get; set; }
    }
}
