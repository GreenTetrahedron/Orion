using Orion.Models.DirectCommunicationModels;
using Orion.Models.GroupModels;
using Orion.Models.MessageModels;
using Orion.Models.ServerTransmissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Server.Groups.Repositories
{
    public interface IGroupRepository
    {
        public Task AddGroup(GroupInformation groupInformation);

        public Task<GroupDTO?> GetGroupById(Guid id);

        public Task<ServerTransmission?> GetGroupMessagesByGroupId(GroupId id);

    }
}
