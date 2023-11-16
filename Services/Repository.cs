using Microsoft.EntityFrameworkCore;
using ODataSample.Data;
using ODataSample.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODataSample.Services
{
    public class Repository : IRepository

    {

        #region Variables

        private readonly TodoContext _context;

        #endregion



        #region Constructors
        public Repository(TodoContext context)
        {
            _context = context;
        }

 



        #endregion

        #region Methods
        public async Task<IEnumerable<TodoItem>> GetItems()
        {
            IQueryable<TodoItem> query = _context.TodoItems;

            var lstItems = (IEnumerable<TodoItem>)await query.ToListAsync();

            if (lstItems.Any())
            {
                return lstItems;
            }


            var lstToDoItems = new List<TodoItem>();

            lstToDoItems.Add(new TodoItem()
            {
                Id = 1,
                Name = "Task # 1",
                IsComplete = false
            });

            lstToDoItems.Add(new TodoItem()
            {
                Id = 2,
                Name = "Task # 2",
                IsComplete = true
            });

            return lstToDoItems;
        }

        public async Task<TodoItem> GetItem(int id)
        {
            var items = await this.GetItems();

            return items.FirstOrDefault(c => c.Id == id);

        }


        //public bool itemExists(int id)
        //{
        //    return this.GetItems().Any(c => c.Id == id);
        //}

        #endregion


    }
}
