using Microsoft.EntityFrameworkCore;
using ODataSample.Models;

namespace ODataSample.Data
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
        }

        public DbSet<TodoItem> TodoItems { get; set; } 
    }
}
