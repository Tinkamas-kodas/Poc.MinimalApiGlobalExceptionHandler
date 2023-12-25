using MinimalApi;
using MinimalApi.v1.Todos;
using Serilog;


namespace MinimalApi
{
    public static class Program
    {

        public static void AddSwagger(this WebApplicationBuilder builder)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
        }


        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateBootstrapLogger();

            try
            {
                var builder = WebApplication.CreateBuilder(args);
                builder.Host.UseSerilog((context, services, configuration) => configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext()
                );

                builder.AddSwagger();

                //add handlers
                builder.AddV1TodosHandlers();


                var app = builder.Build();
                app.UseSwagger();
                app.UseSwaggerUI(t => t.DisplayRequestDuration());


                app.UseErrorHandlingAndLogging();

                //add routes
                app.AddV1TodosRoutes();

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}