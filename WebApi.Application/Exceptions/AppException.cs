namespace WebApi.Application.Common
{
    public class AppException(string message, List<string>? errors = null) : Exception(message)
    {
        public List<string> Errors { get; } = errors ?? [message];
    }
}