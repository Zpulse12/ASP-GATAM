namespace Gatam.Application.Extensions
{
    public class Result<T>
    {
        public bool Success { get; private set; }
        public T? Value { get; private set; }
        public Exception? Exception { get; private set; }

        private Result(bool success, T? value, Exception? exception)
        {
            Success = success;
            Value = value;
            Exception = exception;
        }
        public static Result<T> Ok(T value)
        {
            return new Result<T>(true, value, null);
        }
        public static Result<T> Fail(Exception exception)
        {
            return new Result<T>(false, default, exception);
        }
        public override string ToString()
        {
            return Success ? $"Success: {Value}" : $"Failure: {Exception?.Message}";
        }
    }
}
