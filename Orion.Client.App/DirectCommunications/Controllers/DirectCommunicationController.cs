using Orion.Client.App.Users;
using Orion.Client.Attributes;
using Orion.Models.DirectCommunicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Client.App.DirectCommunications.Controllers
{
    [Controller]
    public class DirectCommunicationController : IDirectCommunicationController
    {
        private readonly App _app;

        public DirectCommunicationController(App app)
        {
            _app = app;
        }

        [Handler("NewDirectCommuniction")]
        public void NewDirectCommunication(DirectCommunicationDTO directCommunicationDTO)
        {
            _app.UserViewModel.DirectCommunicationProfiles.Add(new DirectCommunicationProfileViewModel()
            {
                DirectCommunicationId = directCommunicationDTO.DirectCommunicationId,
                ReceiverProfile = directCommunicationDTO.MemberProfiles
                    .Where(member => member.UserId != App.Current.UserViewModel.UserId)
                    .Select(member => new UserProfileViewModel()
                    {
                        UserId = member.UserId,
                        Username = member.Username
                    })
                    .Single()
            });
        }
    }
}
