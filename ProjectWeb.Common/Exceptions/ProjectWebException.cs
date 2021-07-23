using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Common.Exceptions
{
    public class ProjectWebException : Exception
    {
        public ProjectWebException()
        {
        }
        public ProjectWebException(string message)
            : base(message)
        {
        }
        public ProjectWebException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
