using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageService.Model
{
    public class MessageStatistic
    {

        public int messageCount { get; set; }
        public DateTime timeOfMessage { get; set; }
        public string type { get; set; }

        public MessageStatistic()
        {
        }
    }
}
