using Orion.Logging.LoggingServices;
using Orion.Models.DirectCommunicationModels;
using Orion.Models.GroupModels;
using Orion.Models.ServerTransmissions;
using Orion.Server.Attributes;
using Orion.Server.DirectCommunications.Repositories;
using Orion.Server.Groups.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Server.Groups.Controllers
{
    [Controller]
    public class GroupController : IGroupController
    {
        private readonly IGroupRepository _groupRepository;
        private readonly LoggingService _loggingService;

        public GroupController(IGroupRepository groupRepository, LoggingService loggingService)
        {
            _groupRepository = groupRepository;
            _loggingService = loggingService;
        }

        [Handler("GetGroupMessagesByGroupId")]
        public async Task<ServerTransmission> GetMessagesByGroupId(GroupId id)
        {
            _loggingService.Log($"New group requested id {id.Id}", DateTime.Now);

            return await _groupRepository.GetGroupMessagesByGroupId(id);
        }
    }
}
