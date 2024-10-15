using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Infrastructure.Exceptions
{
    public class EmptyConnectionStringException : Exception
    {
        public EmptyConnectionStringException() : base () { }
        public EmptyConnectionStringException(string message = "Empty connection string") : base(message) { }
        public EmptyConnectionStringException(string atMethodName = "No Method provided", string message = "Empty connection string") : base($"{atMethodName} | ${message}"){ 

        }
    }
}
