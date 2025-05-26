namespace FCG.API.Domain.Models.Response
{
    public class ResponseModel<T>(bool success, int statusCode, string? message = null, T? data = default)
    {
        public bool HasError { get; private set; } = success;
        public int StatusCode { get; private set; } = statusCode;
        public string? Message { get; private set; } = message;
        public T? Data { get; private set; } = data;

        public static ResponseModel<T> Ok(T data) => new(false, 200, null, data);
        public static ResponseModel<T> Created(T data) => new(false, 201, null, data);
        public static ResponseModel<T> NoContent() => new(false, 204);
        public static ResponseModel<T> NotFound(string message) => new(true, 404, message);
        public static ResponseModel<T> BadRequest(string message) => new(true, 400, message);
        public static ResponseModel<T> Error(string message, int code = 500) => new(true, code, message);

        public object? GetResponse() => HasError ? Message : Data;
    }
}
