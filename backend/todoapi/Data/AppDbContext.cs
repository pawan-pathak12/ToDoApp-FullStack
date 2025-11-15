using Microsoft.EntityFrameworkCore;
using todoapi.Entities;

namespace todoapi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<ToDo> Todos { get; set; }

    }
}
