namespace Gatam.WebAppBegeleider.Exceptions
{
    public class ValidationException
    {
        public string Title { get; set; }
        public List<ValidationFailure> Failures { get; set; }
    }

    public class ValidationFailure
    {
        public string PropertyName { get; set; }
        public string ErrorMessage { get; set; }
    }
}
