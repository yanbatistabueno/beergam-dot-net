using Microsoft.AspNetCore.Mvc;

namespace Beergam.Api;

[ApiController]
public abstract class ApiController : ControllerBase
{
    protected IActionResult Ok<T>(T data) =>
        base.Ok(ApiResponse.SuccessResponse(data));

    protected IActionResult Ok<T>(string message, T data) =>
        base.Ok(ApiResponse.SuccessResponse(message, data));

    protected IActionResult Created<T>(string location, T data) =>
        base.Created(location, ApiResponse.SuccessResponse(data));

    protected IActionResult BadRequest(string message) =>
        base.BadRequest(ApiResponse.ErrorResponse(message));

    protected IActionResult NotFound(string message) =>
        base.NotFound(ApiResponse.ErrorResponse(message));

    protected IActionResult Unauthorized(string message) =>
        base.Unauthorized(ApiResponse.ErrorResponse(message));
}

