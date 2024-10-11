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

        public required List<ValidationFailure> _validationFailures { get; set; }

        public FailedValidationException() : base() {}
        public FailedValidationException(string message) : base(message) {}
    }
}
