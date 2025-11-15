using todoapi.Entities;
using TODOAPI.Dtis;

namespace todoapi.Interfaces
{
    public interface ITodoRepository
    {
        Task<IEnumerable<ToDo>> GetAll();
        Task<ToDo?> GetById(int id);
        Task<int> Add(ToDo todo);
        Task<bool> Update(UpdateToDoDto todo, int id);
        Task<bool> Delete(int id);



    }
}
