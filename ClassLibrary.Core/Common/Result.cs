namespace ClassLibrary.Core.Common;

public class Result<T>
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string Message { get; }
    public T? Data { get; }

    private Result(bool isSuccess, string message, T? data = default)
    {
        IsSuccess = isSuccess;
        Message = message;
        Data = data;
    }

    public static Result<T> Success(T? data = default, string message = "") => new(true, message, data);
    public static Result<T> Failure(string message) => new(false, message);
}
