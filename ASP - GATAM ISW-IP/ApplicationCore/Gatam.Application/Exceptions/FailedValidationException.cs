using FluentValidation.Results;

namespace Gatam.Application.Exceptions
{
    public class FailedValidationException : Exception
    {

        public required List<ValidationFailure> _validationFailures { get; set; }

        public FailedValidationException() : base() {}
        public FailedValidationException(string message) : base(message) {}
    }
}
