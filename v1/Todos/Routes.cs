using Microsoft.AspNetCore.Mvc;

namespace MinimalApi.v1.Todos;

public static class Routes
{
    public static void AddV1TodosHandlers(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<GetTodoHandler>();
    }
    public static void AddV1TodosRoutes(this WebApplication app)
    {

        var group = app.MapGroup("/v1/todos")
                .WithProblemDetails(500)
                    .ConvertException( o => o.WithTitle("Internal Server Error"))
                
            ;

        group.MapGet("/", () => Results.Ok(new[] { "todo1", "todo2" }))
            .WithName("GetTodos")
            .Produces<string[]>()
            .WithOpenApi();

        group.MapGet("/{id}", (int id, [FromServices] GetTodoHandler handler) => handler.Handle(id))
            .WithName("GetTodo")
            .Produces<string>()
            .WithProblemDetails(404)
                .ConvertException<NullReferenceException>( o => o
                .WithTitle("todo not found")
                .WithDetail()
                
            )

            .WithOpenApi();

        group.MapPost("/", (string todo) => Results.Ok($"todo{todo}"))
            .WithName("CreateTodo")
            .Produces<string>()
            .WithOpenApi();

        group.MapPut("/{id}", (int id, string todo) => Results.Ok($"{todo}{id}"))
            .WithName("UpdateTodo")
            .Produces<string>()
            .WithOpenApi();

        group.MapDelete("/todos/{id}", (int id) => Results.Ok($"todo{id}"))
            .WithName("DeleteTodo")
            .Produces<string>()
            .WithOpenApi();
    }
}