using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
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
        public Task<GroupDTO?> AddGroup(GroupInformation newGroupInformation);

        public Task<bool> DeleteGroupById(Guid id);

        public Task<GroupDTO?> GetGroupById(Guid id);

        public Task<List<GroupDTO>> GetAllGroups();

        public Task<bool> UpdateGroup(GroupInformation newGroupInformation);

        public Task<ServerTransmission?> GetGroupMessagesByGroupId(GroupId id);

    }
}
