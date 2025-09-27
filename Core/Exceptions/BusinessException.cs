namespace Core.Exceptions
{
    public class BusinessException : Exception
    {
        public List<string> Errors { get; }

        public BusinessException(string message) : base(message)
        {
            Errors = new List<string> { message };
        }

        public BusinessException(List<string> errors)
            : base("One or more validation errors occurred.")
        {
            Errors = errors;
        }
    }
}
