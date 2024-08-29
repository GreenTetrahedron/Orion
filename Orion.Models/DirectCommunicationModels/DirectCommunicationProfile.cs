using Orion.Models.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Models.DirectCommunicationModels
{
    public class DirectCommunicationProfile
    {
        public Guid DirectCommunicationId { get; set; }

        public List<UserProfile> MemberProfiles { get; set; }
    }
}
