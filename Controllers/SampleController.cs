using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using ODataSample.Services;

namespace ODataSample.Controllers
{
    public class SampleController : ODataController
    {
        #region variables
        private readonly IRepository _repository;
        private readonly ILogger _logger;


        #endregion

        #region Constructors
        public SampleController(IRepository repository, ILogger<SampleController> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        #endregion


        #region methods


        [HttpGet("odata_sample/items")]
        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            var result = await _repository.GetItems();
            return this.Ok(result);
        }

        [HttpGet("odata_sample/items/{key}")]
        [HttpGet("odata_sample/items({key})")]
        [EnableQuery]
        public async Task<IActionResult> Get(int key)
        {
            var _msg = $"Invalid item  - {key} ";

            var result = await _repository.GetItem(key);

            if (result == null)
            {
                _logger.LogError(_msg);
                return NotFound(_msg);
            }

            return this.Ok(result);
        }

        #endregion

    }
}
