using Microsoft.AspNetCore.Mvc;

namespace MinimalApi;

public static class ExceptionHandlerMetaDataExtensions
{
    public static ExceptionHandlerMetaData<TException, TProblemDetails> WithTitle<TException,
        TProblemDetails>(this ExceptionHandlerMetaData<TException, TProblemDetails> builder, string title)
        where TException : Exception
        where TProblemDetails : ProblemDetails, new()
    {
        builder.Add((_, _, problemDetails) => problemDetails.Title = title);
        return builder;
    }
    public static ExceptionHandlerMetaData<TException, TProblemDetails> WithDetail<TException,
        TProblemDetails>(this ExceptionHandlerMetaData< TException, TProblemDetails> builder)
        where TException : Exception
        where TProblemDetails : ProblemDetails, new()
    {
        builder.Add((_, exception, problemDetails) => problemDetails.Detail = exception.Message);
        return builder;
    }
    public static ExceptionHandlerMetaData<TException, TProblemDetails> WithStatusCode<TException,
        TProblemDetails>(this ExceptionHandlerMetaData< TException, TProblemDetails> builder, int statusCode)
        where TException : Exception
        where TProblemDetails : ProblemDetails, new()
    {
        builder.Add((_, exception, problemDetails) => problemDetails.Status = statusCode);
        return builder;
    }
}