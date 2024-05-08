namespace ActorManagement.Models
{
    public class Error
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string AdditionalInfo { get; set; }
    }

    public static class ErrorCodes
    {
        public const string NotFound = "NOT_FOUND";
        public const string BadRequest = "BAD_REQUEST";
        // Add more error codes as needed
    }
}
