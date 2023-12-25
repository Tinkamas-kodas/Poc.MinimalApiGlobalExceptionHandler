namespace MinimalApi.v1.Todos;

public class GetTodoHandler
{
    public string Handle(int id)
    {
        if (id < 0) throw new NullReferenceException("id not found");
        if (id > 10) throw new ArgumentException("to big id");
        return $"todo{id}";
    }
}