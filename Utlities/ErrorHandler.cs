using log4net.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ODataSample.Services;
using System;
using System.Data.SqlClient;
using System.Net;
using System.Threading.Tasks;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace ODataSample.Utilities
{
    public class ErrorHandler
    {

        #region variables

        private readonly RequestDelegate _next;

        private readonly ILogger _logger;


        private const string General_Error = "An Unexpected  error has occurred. Please contact the system administrator.";

        #endregion

        #region constructor

        public ErrorHandler(RequestDelegate next, ILogger<ErrorHandler> logger)
        {
            _next = next;
            _logger = logger;

        }
        #endregion

        #region Methods
        public async Task Invoke(HttpContext context)
        {
            try
            {
                var _request = context.Request;

                _logger.LogInformation($"Request: {_request.Method} : {_request.Scheme}:{_request.Host.Value}{_request.Path}");

                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }


        /// <summary>
        /// Handle errors @ each request
        /// </summary>
        /// <param name="oHttpContext"></param>
        /// <param name="oException">application error</param>
        /// <param name="logger">Dependency Injection - logger injected</param>
        /// <returns></returns>
        private async Task HandleExceptionAsync(HttpContext oHttpContext, Exception oException)
        {
            var sErrorMsg = string.Empty;

            if (oException.InnerException == null)    //Message specified @ request,unhandled exeption being thrown 
            {
                sErrorMsg = oException.ToString();
            }
            else
            {
                sErrorMsg = oException.InnerException.ToString();
            }

            //log error @ log4net
            _logger.LogError(sErrorMsg);

            int iStatusCode = (int)HttpStatusCode.InternalServerError;
            string sMessage = oException.Message.Replace("\r\n", " ");

            sMessage = General_Error;

            //type of  exption thrown:
            switch (oException)
            {
                case SqlException     //any database specific sql ex, e.g. network error connecting
                AmbiguousMatchException:
                    break;
                case DataValidationException
                     BadRequestException:
                    iStatusCode = (int)HttpStatusCode.BadRequest;
                    sMessage = oException.Message;
                    break;
                case DbUpdateException:
                    break;
            }

            var oErrorMessageObject = new {  Status = iStatusCode, Message = sMessage};
            var oResponse = oHttpContext.Response;

            oResponse.ContentType = "application/json";
            oResponse.StatusCode = iStatusCode;

            //write  out error message
            sMessage = JsonConvert.SerializeObject(oErrorMessageObject);
            await oResponse.WriteAsync(sMessage);
        }




        #endregion
    }
}
