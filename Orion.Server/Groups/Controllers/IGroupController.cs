using Orion.Models.DirectCommunicationModels;
using Orion.Models.GroupModels;
using Orion.Models.MessageModels;
using Orion.Models.ServerTransmissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Server.Groups.Controllers
{
    public interface IGroupController
    {
        public Task<ServerTransmission> GetMessagesByGroupId(GroupId id);
    }
}
