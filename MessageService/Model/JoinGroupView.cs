using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageService.Model
{
    public class JoinGroupView
    {
        public List<string> groups { get; set; }
        public string connectionId { get; set; }

        public JoinGroupView()
        {
        }

        public JoinGroupView(List<string> groups, string connectionId)
        {
            this.groups = groups;
            this.connectionId = connectionId;
        }
    }
}
