using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Infrastructure.Exceptions
{
    public class MissingEnvironmentVariableException : Exception
    {
        public MissingEnvironmentVariableException() : base ("Missing environment variable."){ }

        public MissingEnvironmentVariableException(string message) : base(message) { }
        public MissingEnvironmentVariableException(string message, Exception inner) : base(message, inner){}

    }
}
