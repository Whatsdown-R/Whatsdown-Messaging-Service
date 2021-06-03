using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageService.Exceptions
{
    public class LengthException : Exception
    {
        public LengthException(string message) : base(message) { }
    }
}
