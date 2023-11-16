using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Threading.Tasks;
using ODataSample.Data;
using ODataSample.Models;
using ODataSample.Services;

namespace ODataSample.Controllers
{
    [Produces("application/json")]
    [ODataRouteComponent]
    public class ApplicationController : ODataController
    {
        #region Variables

        private readonly IApplicationRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        private const string  _NotFound = "Invalid application  - ID";

        #endregion

        #region Constructors
        public ApplicationController(IApplicationRepository applicationsRepository, IMapper mapper,
            ILogger<ApplicationController> logger)
        {
            _repository = applicationsRepository;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion



        [HttpGet("odata/applications")]
        [HttpGet("odata/applications/$count")]
        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            try
            {
                var results = await _repository.GetApplications();

                var oMappedResults = _mapper.Map<ApplicationModel[]>(results);

                return Ok(oMappedResults);
            }
            catch (Exception ex)
            {
                throw new InternalErrorException(ex.Message);
            }


        }


        [EnableQuery]
        [HttpGet("odata/applications({key})")]
        [HttpGet("odata/applications/{key}")]
        public async Task<IActionResult> Get([FromODataUri]  int key)
        {
            try
            {


                if (!_repository.appExists(key))
                {               
                    var _msg = $"{_NotFound}:{key} ";

                    _logger.LogError(_msg);
                    return NotFound(_msg);
                }

                var result = await _repository.GetApplication(key);

                return Ok(_mapper.Map<ApplicationModel>(result));
            }
            catch (Exception ex)
            {
                throw new InternalErrorException(ex.Message);
            }
        }



        [EnableQuery]
        [HttpPost("odata/applications")]
        public async Task<IActionResult> AddApplication([FromBody] ApplicationModel model)
        {


            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Application repoApplication = _mapper.Map<Application>(model);

                await _repository.SaveApplication(repoApplication);

                return this.Ok();

            }

            catch (DbUpdateConcurrencyException)
            {

                throw new DataValidationException();

            }
        }


        [EnableQuery]
        [HttpDelete("odata/applications/{key}")]
        public async Task<IActionResult> DeleteApplication([FromODataUri]  int key)
        {

            try
            {


                if (!_repository.appExists(key))
                {
                    var _msg = $"{_NotFound}:{key} ";

                    _logger.LogError(_msg);
                    return  NotFound(_msg);

                }

                await _repository.DeleteApplication(key);

                return StatusCode((int)HttpStatusCode.NoContent);
            }

            catch (DbUpdateConcurrencyException)
            {

                throw new DataValidationException();

            }
        }

        [EnableQuery]
        [HttpPut("odata/applications/{id:int}")]
        public  async Task<IActionResult> UpdateApplication(int id,  [FromBody] ApplicationModel model)
        {
            if (id != model.ID)
            {
                return BadRequest("Cant update application - ID mismatch");
            }

            if (!_repository.appExists(id))
            {
                var _msg = $"{_NotFound}:{id} ";

                _logger.LogError(_msg);
                return NotFound(_msg);

            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Application repo = _mapper.Map<Application>(model); //data

                await _repository.SaveApplication(repo);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_repository.appExists(id))
                {
                    return BadRequest($"{ _NotFound}:{id} ");
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(StatusCodes.Status204NoContent);

        }


        [EnableQuery]
        [HttpPatch("odata/applications/{id:int}")]
        public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<ApplicationModel> patchDocument)
        {

            if (!_repository.appExists(id))
            {
                var _msg = $"{_NotFound}:{id} ";

                _logger.LogError(_msg);
                return NotFound(_msg);

            }

            try
            {

                var repo = await _repository.GetApplication(id);
                var toPatch = _mapper.Map<ApplicationModel>(repo);
                patchDocument.ApplyTo(toPatch, ModelState);

                if (!TryValidateModel(toPatch))
                {
                    return ValidationProblem(ModelState);
                }

                _mapper.Map(toPatch, repo);
                await _repository.SaveApplication(repo);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_repository.appExists(id))
                {
                    return BadRequest($"{_NotFound}:{id}");
                }
                else
                {
                    throw;
                }
            }

            return StatusCode((int)HttpStatusCode.NoContent);
        }

       // public override ActionResult ValidationProblem(
       //[ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
       // {
       //     var options = HttpContext.RequestServices
       //         .GetRequiredService<IOptions<ApiBehaviorOptions>>();
       //     return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
       // }


    }
}

