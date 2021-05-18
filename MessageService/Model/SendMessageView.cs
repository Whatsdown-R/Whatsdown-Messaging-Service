using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageService.Model
{
    public class SendMessageView
    {
        public string profileId { get; set; }
        public  string identificationCode { get; set; }
        public string message { get; set; }
        public string type { get; set; }
        public DateTime date { get; set; }

        public SendMessageView(string profileId, string identificationCode, string message, string type, DateTime date)
        {
            this.profileId = profileId;
            this.identificationCode = identificationCode;
            this.message = message;
            this.type = type;
            this.date = date;
        }

        public SendMessageView()
        {
        }
    }
}
