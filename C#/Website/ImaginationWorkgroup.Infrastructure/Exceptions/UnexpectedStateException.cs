using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImaginationWorkgroup.Infrastructure.Exceptions
{
    public class UnexpectedStateException : Exception
    {
        public UnexpectedStateException(string message) : base(message)
        {
        }
    }
}
