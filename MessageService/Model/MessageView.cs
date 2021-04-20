using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageService.Model
{
    public class MessageView
    {
        public string SenderId { get;  set; }
        public string IdentificationCode{ get;  set; }
        public string Message { get;  set; }
        public string Type { get;  set; }
        public DateTime date { get; set; }

        public MessageView()
        {
        }
        public MessageView(string senderId, string identificationCode, string message, string type)
        {
            this.SenderId = senderId;
            this.IdentificationCode = identificationCode;
            this.Message = message;
            this.Type = type;
            this.date = DateTime.Now;
        }

     
    }
}
