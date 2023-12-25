namespace MinimalApi;

public static class RouteHandlerBuilderExtensions
{
   public static ExceptionHandlerMetaDataBuilder<TBuilder> WithProblemDetails<TBuilder>(this TBuilder builder, int statusCode)
        where TBuilder : IEndpointConventionBuilder
    {
        return new ExceptionHandlerMetaDataBuilder<TBuilder>(builder, statusCode);
    }

}