using Microsoft.AspNetCore.Mvc;

namespace MinimalApi;

public class ExceptionHandlerMetaData<TException, TProblemDetails>() : IExceptionHandlerMetaData
    where TException : Exception
    where TProblemDetails : ProblemDetails, new()
{

    private readonly List<Action<HttpContext, Exception, TProblemDetails>> decorators = [];
    public ExceptionHandlerMetaData<TException, TProblemDetails> Add(Action<HttpContext, Exception, TProblemDetails> decorator)
    {
        decorators.Add(decorator);
        return this;
    }
    public ProblemDetails Build(HttpContext context, Exception exception)
    {
        var problemDetails = new TProblemDetails();
        foreach (var action in decorators)
        {
            action(context, exception, problemDetails);
        }
        return problemDetails;
    }

    public Type ExceptionType => typeof(TException);
}

