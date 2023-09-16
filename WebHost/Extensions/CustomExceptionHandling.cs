using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;

namespace WebHost.Extensions;

public static class CustomExceptionHandling
{
    private const string DefaultErrorMessage = "Internal Server Error.";
    public static void ApiExceptionHandling(this WebApplication app)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    var errorDetails = new
                    {
                        Message = string.IsNullOrWhiteSpace(contextFeature.Error.Message)
                            ? DefaultErrorMessage
                            : contextFeature.Error.Message
                    };
                    await context.Response.WriteAsync(JsonSerializer.Serialize(errorDetails));
                }
            });
        });
    }
}