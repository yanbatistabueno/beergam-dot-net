using System.Text.Json.Serialization;
namespace Beergam.Api;

public interface IBaseResponse<T> {
    public string Message { get; set; }
    public bool Success { get; set; }
}

public interface IApiResponse<T> : IBaseResponse<T> {
    public T Data { get; set; }
}

public interface IPagination {
    public bool HasNext { get; set; }
    public bool HasPrev { get; set; }
    public int Page { get; set; }
    public int PerPage { get; set; }
    public int TotalPages { get; set; }
}

public interface IPaginatedResponse<T> : IBaseResponse<T> {
    public IPagination Pagination { get; set; }
    public String KeyName { get; set; }
    [JsonExtensionData]
    public Dictionary<string, object> Extra { get; set; }
}

public class ApiResponse<T> : IApiResponse<T> {
    public String Message { get; set; }
    public bool Success { get; set; }
    public T Data { get; set; }
    public ApiResponse(String message, bool success, T data) {
        Message = message;
        Success = success;
        Data = data;
    }
    public static ApiResponse<T> SuccessResponse(T data) {
        return new ApiResponse<T>("Sucesso", true, data);
    }
    public static ApiResponse<T> SuccessResponse(T data, String message) {
        return new ApiResponse<T>(message, true, data);
    }
    public static ApiResponse<T> ErrorResponse(String message) {
        return new ApiResponse<T>(message, false, default!);
    }
}

public static class ApiResponse {
    public static ApiResponse<T> SuccessResponse<T>(T data) =>
        ApiResponse<T>.SuccessResponse(data);

    public static ApiResponse<T> SuccessResponse<T>(string message, T data) =>
        ApiResponse<T>.SuccessResponse(data, message);

    public static ApiResponse<object?> ErrorResponse(string message) =>
        ApiResponse<object?>.ErrorResponse(message);
}

public class PaginatedResponse<T> : IPaginatedResponse<T> {
    public String Message { get; set; }
    public bool Success { get; set; }
    public IPagination Pagination { get; set; }
    public String KeyName { get; set; }
    [JsonExtensionData]
    public Dictionary<string, object> Extra { get; set; } = new();
    public PaginatedResponse(String message, bool success, IPagination pagination, String keyname, T data) {
        Message = message;
        Success = success;
        Pagination = pagination;
        KeyName = keyname;
        Extra[keyname] = data;
    }
    public static PaginatedResponse<T> SuccessResponse(T data, IPagination pagination, String keyname)
    {
        return new PaginatedResponse<T>("Sucesso", true, pagination, keyname, data);
    }
    public static PaginatedResponse<T> SuccessResponse(String message, T data, IPagination pagination, String keyname)
    {
        return new PaginatedResponse<T>(message, true, pagination, keyname, data);
    }
    public static PaginatedResponse<T> ErrorResponse(String message)
    {
        return new PaginatedResponse<T>(message, false, null, "", default!);
    }
}

public class PaginatedResponse {
    public static PaginatedResponse<T> SuccessResponse<T>(T data, IPagination pagination, String keyname) =>
        PaginatedResponse<T>.SuccessResponse(data, pagination, keyname);
    public static PaginatedResponse<T> SuccessResponse<T>(String message, T data, IPagination pagination, String keyname) =>
        PaginatedResponse<T>.SuccessResponse(message, data, pagination, keyname);
    public static PaginatedResponse<T> ErrorResponse<T>(String message) =>
        PaginatedResponse<T>.ErrorResponse(message);
}