namespace BankAccountApi.Security.Exceptions;

public abstract class ApiException : Exception
{
    protected ApiException(int statusCode, string title, string message)
        : base(message)
    {
        StatusCode = statusCode;
        Title = title;
    }

    public int StatusCode { get; }
    public string Title { get; }
}

public sealed class BadRequestException : ApiException
{
    public BadRequestException(string message)
        : base(StatusCodes.Status400BadRequest, "Bad request", message)
    {
    }
}

public sealed class UnauthorizedException : ApiException
{
    public UnauthorizedException(string message)
        : base(StatusCodes.Status401Unauthorized, "Unauthorized", message)
    {
    }
}

public sealed class NotFoundException : ApiException
{
    public NotFoundException(string message)
        : base(StatusCodes.Status404NotFound, "Resource not found", message)
    {
    }
}

public sealed class ConflictException : ApiException
{
    public ConflictException(string message)
        : base(StatusCodes.Status409Conflict, "Conflict", message)
    {
    }
}
