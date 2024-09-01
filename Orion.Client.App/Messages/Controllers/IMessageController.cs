using Orion.Models.MessageModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Client.App.Messages.Controllers
{
    public interface IMessageController
    {
        public void NewDirectMessage(DirectMessageDTO directMessage);
    }
}
