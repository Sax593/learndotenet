using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Data;
using TodoApp.Model;

namespace TodoApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TodoController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/todo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Todo>>> GetTodos()
        {
            return await _context.Todos.ToListAsync();
        }

        // GET: api/todo/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Todo>> GetTodoById(int id)
        {
            var todo = await _context.Todos.FindAsync(id);

            if (todo == null)
            {
                return NotFound();
            }

            return todo;
        }

        // POST: api/todo
        [HttpPost]
        public async Task<ActionResult<Todo>> CreateTodo([FromBody] Todo newTodo)
        {
            if (newTodo == null || string.IsNullOrWhiteSpace(newTodo.Title))
            {
                return BadRequest("Le titre ne peut pas être vide.");
            }

            _context.Todos.Add(newTodo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoById), new { id = newTodo.Id }, newTodo);
        }

        // PUT: api/todo/1
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodo(int id, [FromBody] Todo updatedTodo)
        {
            if (updatedTodo == null || string.IsNullOrWhiteSpace(updatedTodo.Title))
            {
                return BadRequest("Le titre ne peut pas être vide.");
            }

            var todo = await _context.Todos.FindAsync(id);
            if (todo == null)
            {
                return NotFound();
            }

            todo.Title = updatedTodo.Title;
            todo.IsDone = updatedTodo.IsDone;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/todo/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null)
            {
                return NotFound();
            }

            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
