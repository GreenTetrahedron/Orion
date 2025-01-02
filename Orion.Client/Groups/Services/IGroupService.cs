using Orion.Client.Subscriptions;
using Orion.Models.ServerTransmissions.Results.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Client.Groups.Services
{
    public interface IGroupService
    {
        public Task<Subscriptable<GetMessageMessages>> GetMessagesByGroupId(Guid id);
    }
}
