using Microsoft.AspNetCore.Mvc;

namespace MinimalApi;

public interface IExceptionHandlerMetaData
{
    ProblemDetails Build(HttpContext context, Exception exception);
    Type ExceptionType { get; }
}