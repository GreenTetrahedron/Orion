using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Models.MessageModels
{
    public class GroupMessageDTO
    {
        public Guid GroupId { get; set; }

        public MessageDTO Message { get; set; }
    }
}
