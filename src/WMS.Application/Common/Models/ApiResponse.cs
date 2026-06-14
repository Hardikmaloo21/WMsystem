// WMS.Application/Common/Models/ApiResponse.cs
namespace WMS.Application.Common.Models;

public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public object? Errors { get; set; }
    public PaginationMeta? Pagination { get; set; }

    public static ApiResponse<T> Success(T data, string message = "Success", int statusCode = 200)
        => new() { IsSuccess = true, StatusCode = statusCode, Message = message, Data = data };

    public static ApiResponse<T> Fail(string message, int statusCode = 400, object? errors = null)
        => new() { IsSuccess = false, StatusCode = statusCode, Message = message, Errors = errors };
}

public class PaginationMeta
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
}