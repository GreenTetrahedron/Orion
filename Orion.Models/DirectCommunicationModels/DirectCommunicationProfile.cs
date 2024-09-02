using Orion.Models.UserModels;

namespace Orion.Models.DirectCommunicationModels
{
    public class DirectCommunicationProfile
    {
        public Guid DirectCommunicationId { get; set; }

        public List<UserProfile> MemberProfiles { get; set; }
    }
}
