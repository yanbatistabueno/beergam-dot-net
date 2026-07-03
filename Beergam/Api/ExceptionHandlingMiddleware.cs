using System.Text.Json;
using Beergam.Api;

namespace Beergam.Api;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception for {Method} {Path}",
                context.Request.Method, context.Request.Path);

            await WriteErrorResponse(context, ex);
        }
    }

    private static async Task WriteErrorResponse(HttpContext context, Exception ex)
    {
        if (context.Response.HasStarted)
            return;

        var (status, message) = MapException(ex);

        context.Response.Clear();
        context.Response.StatusCode = status;
        context.Response.ContentType = "application/json";

        var payload = ApiResponse.ErrorResponse(message);

        var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }

    private static (int status, string message) MapException(Exception ex) => ex switch
    {
        UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Não autorizado."),
        KeyNotFoundException        => (StatusCodes.Status404NotFound, "Recurso não encontrado."),
        ArgumentException           => (StatusCodes.Status400BadRequest, ex.Message),
        _                           => (StatusCodes.Status500InternalServerError, "Ocorreu um erro interno.")
    };
}