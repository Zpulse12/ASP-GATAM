using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Application.Exceptions
{
    public class FailedValidationException : Exception
    {

        public List<ValidationFailure> _validationFailures;

        public FailedValidationException(List<ValidationFailure> failures) : base() { _validationFailures = failures; }
        public FailedValidationException(string message) : base(message) { _validationFailures = new List<ValidationFailure>(); }
    }
}
