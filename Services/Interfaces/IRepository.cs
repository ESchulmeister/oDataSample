using ODataSample.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ODataSample.Services
{
    public interface IRepository
    {

        public Task<IEnumerable<TodoItem>> GetItems();

         public Task<TodoItem> GetItem(int id);

    }
}
