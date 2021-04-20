using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageService.Model
{
    public class DeleteMessageView
    {
        public string senderId { get;  set; }
        public string messageId { get;  set; }

        public DeleteMessageView()
        {
        }
        public DeleteMessageView(string senderId, string messageId)
        {
            this.senderId = senderId;
            this.messageId = messageId;
        }

      
    }
}
