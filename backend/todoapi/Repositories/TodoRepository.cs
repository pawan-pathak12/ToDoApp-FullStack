using Microsoft.EntityFrameworkCore;
using todoapi.Data;
using todoapi.Entities;
using todoapi.Interfaces;
using TODOAPI.Dtis;

namespace todoapi.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly AppDbContext _context;

        public TodoRepository(AppDbContext dbContext)
        {
            this._context = dbContext;
        }
        public async Task<IEnumerable<ToDo>> GetAll()
        {
            return await _context.Todos.ToListAsync();
        }

        public async Task<ToDo?> GetById(int id)
        {
            return await _context.Todos.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<int> Add(ToDo todo)
        {
            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();
            return todo.Id;
        }

        public async Task<bool> Update(UpdateToDoDto todo, int id)
        {
            var existingTodo = await _context.Todos.FindAsync(id);
            if (existingTodo == null) return false;

            existingTodo.IsCompleted = todo.IsCompleted;
            //   existingTodo.Title = todo.Title; // if you want to allow updating title too

            _context.Todos.Update(existingTodo);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(int id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null) return false;

            _context.Todos.Remove(todo);
            return await _context.SaveChangesAsync() > 0;
        }


    }
}
