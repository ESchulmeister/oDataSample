using ODataSample.Data;
using ODataSample.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ODataSample.Services
{
    public interface IApplicationRepository
    {
       bool appExists(int id);

        public Task<IEnumerable<Application>> GetApplications();

       public Task<Application> GetApplication(int id);

       Task SaveApplication(Application repoApplication);

        Task DeleteApplication(int id);

    }
}
