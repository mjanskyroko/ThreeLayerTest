namespace TestWebApp.Application.Internal.Exceptions
{
    public class ServiceValidationException : ApplicationException
    {
        public ServiceValidationException(Dictionary<string, string[]> errors)
        {
            Title = "Validation errors occured.";
            Detail = "Check the errors for more details";
            Errors = errors;
        }

        public string Title { get; }

        public string Detail { get; }

        public Dictionary<string, string[]> Errors { get; }
    }
}
