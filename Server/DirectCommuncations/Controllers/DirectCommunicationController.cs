using Orion.Models.ClientTransmissions;
using Orion.Models.ServerTransmissions.Results;
using Orion.Server.Attributes;
using Orion.Server.DirectCommunications.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Server.DirectCommuncations.Controllers
{
    [Controller]
    public class DirectCommunicationController : IDirectCommunicationController
    {
        private readonly IDirectCommunicationRepository _directCommunicationRepository;

        public DirectCommunicationController(IDirectCommunicationRepository directCommunicationRepository)
        {
            _directCommunicationRepository = directCommunicationRepository;
        }

        [Handler("NewDirectCommunication")]
        public async Task<ServerResult<DirectCommunicationMessages>?> AddDirectCommunication(NewDirectCommunicationDTO newDirectCommunication)
        {
            return await _directCommunicationRepository.AddDirectCommunication(newDirectCommunication);
        }
    }
}
