namespace Api.Wrappers
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public List<string>? Errors { get; set; }

        public ApiResponse(int statusCode, string message, List<string>? errors = null)
        {
            StatusCode = statusCode;
            Message = message;
            Errors = errors;
        }

        public static ApiResponse Response(int statusCode, string message, List<string>? errors = null)
        {
            return new ApiResponse(statusCode, message, errors);
        }
    }
}
