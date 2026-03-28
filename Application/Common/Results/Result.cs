namespace Application.Common.Results;

public enum ErrorTypes
{
    BadRequest = 400,
    NotFound = 404,
    Conflict = 409,
    Error = 500
}

public record Result(bool Success, ErrorTypes? ErrorType = null, string? ErrorMessage = null)
{
    public static Result Ok() => new(true);

    public static Result BadRequest(string errorMessage) => new(false, ErrorTypes.BadRequest, errorMessage);
    public static Result NotFound(string errorMessage) => new(false, ErrorTypes.NotFound, errorMessage);
    public static Result Conflict(string errorMessage) => new(false, ErrorTypes.Conflict, errorMessage);
    public static Result Error(string errorMessage = "An unexpected error occurred.") => new(false, ErrorTypes.Error, errorMessage);
}

public record Result<T>(bool Success, T? Value = default, ErrorTypes? ErrorType = null, string? ErrorMessage = null)
{
    public static Result<T> Ok(T value) => new(true, value);

    public static Result<T> BadRequest(string errorMessage) => new(false, default, ErrorTypes.BadRequest, errorMessage);
    public static Result<T> NotFound(string errorMessage) => new(false, default, ErrorTypes.NotFound, errorMessage);
    public static Result<T> Conflict(string errorMessage, T? value = default) => new(false, value, ErrorTypes.Conflict, errorMessage);
    public static Result<T> Error(string errorMessage = "An unexpected error occurred.") => new(false, default, ErrorTypes.Error, errorMessage);
}
