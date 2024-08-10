using Server.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Messages
{
    public class Message
    {
        public Guid MessageId { get; set; }

        public string Content { get; set; }

        public Guid SenderId { get; set; }
    }
}
