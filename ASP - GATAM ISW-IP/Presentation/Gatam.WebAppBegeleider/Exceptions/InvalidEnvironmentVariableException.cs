namespace Gatam.Infrastructure.Exceptions
{
    internal class InvalidEnvironmentVariableException : Exception
    {
        public InvalidEnvironmentVariableException() { }
        public InvalidEnvironmentVariableException(string message) : base (message){ }
        public InvalidEnvironmentVariableException (string message, Exception inner) : base(message, inner) { }
    }
}
