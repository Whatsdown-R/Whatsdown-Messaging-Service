using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageService.Model
{
    public class RecentMessageView
    {
        public string mostRecentMessage { get; set; }
        public string identificationCode { get; set; }

        public string senderId { get; set; }
        public DateTime date { get; set; }

        public RecentMessageView()
        {
        }

        public RecentMessageView(string mostRecentMessage, string identificationCode, string senderId, DateTime date)
        {
            this.mostRecentMessage = mostRecentMessage;
            this.identificationCode = identificationCode;
            this.senderId = senderId;
            this.date = date;
        }
    }
}
