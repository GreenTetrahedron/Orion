using Orion.Models.UserModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Models.MessageModels
{
    public class MessageDTO
    {
        public Guid MessageId { get; set; }

        public UserProfile SenderProfile { get; set; }

        public string Content { get; set; }
    }
}
