using Orion.Models.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Models.GroupModels
{
    public class GroupInformation
    {
        public Guid GroupId { get; set; }

        public string Name { get; set; }

        public List<UserProfile> MemberProfiles { get; set; }
    }
}
