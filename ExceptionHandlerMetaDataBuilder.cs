using Microsoft.AspNetCore.Mvc;

namespace MinimalApi;

public class ExceptionHandlerMetaDataBuilder<TBuilder>(TBuilder builder, int statusCode)
    where TBuilder : IEndpointConventionBuilder
{
    public  TBuilder ConvertException<TException, TProblemDetails>( Action<ExceptionHandlerMetaData< TException,TProblemDetails>> options) where TException : Exception where TProblemDetails : ProblemDetails, new()
    {

        builder.WithMetadata(new ProducesResponseTypeMetadata(statusCode, typeof(TProblemDetails)));

        var metaDataBuilder= new ExceptionHandlerMetaData<TException, TProblemDetails>()
            .WithStatusCode(statusCode);

        options.Invoke(metaDataBuilder);
        builder.WithMetadata(metaDataBuilder);
        
        return builder;
    }
    public TBuilder ConvertException<TException>(Action<ExceptionHandlerMetaData<TException, ProblemDetails>> options)
        where TException : Exception
        => ConvertException<TException, ProblemDetails>(options);
    public TBuilder ConvertException(Action<ExceptionHandlerMetaData<Exception, ProblemDetails>> options)
        => ConvertException<Exception, ProblemDetails>(options);

}