using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Models.MessageModels
{
    public class NewMessage
    {
        public Guid SenderId { get; set; }

        public string Content { get; set; }
    }
}
