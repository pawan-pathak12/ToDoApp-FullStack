using Microsoft.AspNetCore.Mvc;
using todoapi.Entities;
using todoapi.Interfaces;
using TODOAPI.Dtis;

namespace todoapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ITodoRepository _repository;

        public TodoController(ITodoRepository repository)
        {
            _repository = repository;
        }

        // GET: api/todo
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var todos = await _repository.GetAll();
            return Ok(todos);
        }

        // GET: api/todo/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var todo = await _repository.GetById(id);
            if (todo == null) return NotFound();
            return Ok(todo);
        }

        // POST: api/todo
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateToDoDto todoDto)
        {
            var todo = new ToDo
            {
                Title = todoDto.Title,
                IsCompleted = todoDto.IsCompleted
            };
            var id = await _repository.Add(todo);
            return CreatedAtAction(nameof(GetById), new { id }, todo);
        }

        // PUT: api/todo/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateToDoDto todo)
        {
            var todos = await _repository.GetById(id);
            if (todos == null)
            {
                return NotFound();
            }

            var success = await _repository.Update(todo, id);
            if (!success) return NotFound();

            return Ok("Updated Successfully");
        }

        // DELETE: api/todo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _repository.Delete(id);
            if (!success) return NotFound();

            return NoContent();
        }


    }
}
