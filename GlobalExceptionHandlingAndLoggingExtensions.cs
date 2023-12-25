using Serilog;

namespace MinimalApi;

public static class GlobalExceptionHandlingAndLoggingExtensions
{
    public static void UseErrorHandlingAndLogging(this WebApplication app)
    {
        app.Use(async (context, next) =>
        {
            var start = DateTime.Now;
            var currentEndpoint = context.GetEndpoint();

            if (currentEndpoint is null)
            {
                await next(context);
                return;
            }

            try
            {
                await next(context);
                Log.Information("endpoint {endpoint} return {statusCode}  in {elapsed} ms",
                    currentEndpoint.DisplayName, context.Response.StatusCode,
                    (DateTime.Now - start).TotalMilliseconds);
            }
            catch (Exception ex)
            {
                var exceptionType = ex.GetType();
                var handler = currentEndpoint.Metadata
                    .Where(t => t is IExceptionHandlerMetaData)
                    .Cast<IExceptionHandlerMetaData>()
                    .Reverse()
                    .FirstOrDefault(t => exceptionType.IsAssignableTo(t.ExceptionType));
                if (handler == null)
                {
                    //did not find a handler - rethrow
                    Log.Error(ex, "endpoint {endpoint} throw unhandled error. Elapsed {elapsed} ms",
                        currentEndpoint.DisplayName, (DateTime.Now - start).TotalMilliseconds);
                    throw;
                }

                var problemDetails = handler.Build(context, ex);
                context.Response.StatusCode = problemDetails.Status ?? 500;
                Log.Error(ex, "endpoint {endpoint} return {statusCode} in {elapsed} ms",
                    currentEndpoint.DisplayName, context.Response.StatusCode,
                    (DateTime.Now - start).TotalMilliseconds);
                await context.Response.WriteAsJsonAsync(problemDetails);
            }

        });
    }
}