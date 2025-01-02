using Orion.Client.Subscriptions;
using Orion.Models.GroupModels;
using Orion.Models.MessageModels;
using Orion.Models.ServerTransmissions.Results.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Client.Groups.Services
{
    public class GroupService : IGroupService
    {
        private readonly IClientService _clientService;

        public GroupService(IClientService clientService)
        {
            _clientService = clientService;
        }

        public async Task<Subscriptable<GetMessageMessages>> GetMessagesByGroupId(Guid id)
        {
            return await _clientService.TransmitDataOfTopic<GetMessageMessages>(new GroupId() { Id = id }, "GetGroupMessagesByGroupId");
        }
    }
}
