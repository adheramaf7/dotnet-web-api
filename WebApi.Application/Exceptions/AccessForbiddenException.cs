namespace WebApi.Application.Exceptions
{
    public class AccessForbiddenException(string message = "Access forbidden") : Exception(message)
    {
    }
}