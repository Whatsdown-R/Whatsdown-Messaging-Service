using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageService.Model
{
    public class Message
    {
        public string Id { get; set; }
        public string senderId { get; private set; }
        public string identificationCode { get; private set; }
        public string message { get; private set; }
        public string type { get; private set; }
        public DateTime date { get; private set; }

        public Message(string id, string senderId, string identificationCode, string message, string type, DateTime date)
        {
            Id = id;
            this.senderId = senderId;
            this.identificationCode = identificationCode;
            this.message = message;
            this.type = type;
            this.date = date;
        }

      
    }
}
