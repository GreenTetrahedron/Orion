using Orion.Client.App.Users.Models;
using Orion.Client.Attributes;
using Orion.Models.DirectCommunicationModels;
using Orion.Models.ServerTransmissions.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Client.App.DirectCommunications.Controllers
{
    [Controller]
    public class DirectCommunicationController
    {
        private readonly App _app;

        public DirectCommunicationController(App app)
        {
            _app = app;
        }

        [Handler("NewDirectCommunication")]
        public async Task NewDirectCommunication(ServerResult serverResult)
        {
            if (serverResult?.Data == null)
                return;

            var directCommunicationDTO = (DirectCommunicationDTO)serverResult.Data;

            var directCommunication = new Models.DirectCommunication
            {
                DirectCommunicationId = directCommunicationDTO.DirectCommunicationId,
                ReceiverProfile = directCommunicationDTO.MemberProfiles
                    .Where(member => member.UserId != _app.CurrentUser.UserProfile.UserId)
                    .Select(member => new UserProfile
                    {
                        UserId = member.UserId,
                        Username = member.Username
                    })
                    .Single()
            };

            _app.CurrentUser.DirectCommunications.Add(directCommunication);

            _app.CurrentUser.DirectCommunicationListViewModel.AddItem(directCommunication);
        }
    }
}
