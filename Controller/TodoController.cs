using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class TodoController : ControllerBase
{
    private static List<Todo> todos = new List<Todo>
    {
        new Todo { Id = 1, Title = "Apprendre .NET", IsDone = false },
        new Todo { Id = 2, Title = "Création d'un CRUD", IsDone = false }
    };

    [HttpGet]
    public ActionResult<IEnumerable<Todo>> GetTodos()
    {
        return Ok(todos);
    }

    [HttpGet("{id}")]
    public ActionResult<Todo> GetTodoById(int id)
    {
        var todo = todos.FirstOrDefault(t => t.Id == id);
        if (todo == null)
        {
            return NotFound();
        }
        return Ok(todo);
    }

    [HttpPost]
    public ActionResult<Todo> CreateTodo([FromBody] Todo newTodo)
    {
        if (newTodo == null || string.IsNullOrWhiteSpace(newTodo.Title))
        {
            return BadRequest("Le titre ne peut pas être vide.");
        }

        newTodo.Id = todos.Max(t => t.Id) + 1;
        todos.Add(newTodo);
        return CreatedAtAction(nameof(GetTodoById), new { id = newTodo.Id }, newTodo);
    }

    [HttpPut("{id}")]
    public ActionResult UpdateTodo(int id, [FromBody] Todo updatedTodo)
    {
        if (updatedTodo == null || string.IsNullOrWhiteSpace(updatedTodo.Title))
        {
            return BadRequest("Le titre ne peut pas être vide.");
        }

        var todo = todos.FirstOrDefault(t => t.Id == id);
        if (todo == null)
        {
            return NotFound();
        }

        todo.Title = updatedTodo.Title;
        todo.IsDone = updatedTodo.IsDone;
        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteTodo(int id)
    {
        var todo = todos.FirstOrDefault(t => t.Id == id);
        if (todo == null)
        {
            return NotFound();
        }

        todos.Remove(todo);
        return NoContent();
    }
}