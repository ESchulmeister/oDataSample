using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ODataSample.Data;
using ODataSample.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ODataSample.Services
{
    public class ApplicationRepository : IApplicationRepository

    {

        #region Variables

        private readonly SampleContext _context;

        #endregion

        #region Constructors
        public ApplicationRepository(SampleContext context)
        {
            _context = context;
        }
        



        #endregion

        #region Methods
        public async Task<IEnumerable<Application>> GetApplications()
        {

           var lstPermissions = new List<AppPermission>();

            IQueryable <Application> applications = _context.Applications
                .Where(o => o.AppActive == true)
                .Include(x => x.AppPermissions).AsNoTracking();

            var lstAllPermissions = _context.AppPermissions; // will populate the AppPermissions property

            return await applications.ToListAsync();
        }

        public async Task<Application> GetApplication(int id)
        {
            var lstApplications = await this.GetApplications();

            return lstApplications.FirstOrDefault(oApplication => oApplication.AppId == id);

            //IQueryable<Application> query = _context.Applications.Where(c => c.AppId == id);

            //return await query.FirstOrDefaultAsync();
        }


        public bool appExists(int id)
        {
            return _context.Applications.Any(e => e.AppId == id);
        }


        /// <summary>
        /// </summary>
        /// <param name="repoApplication"></param>
        /// <returns></returns>
        public async Task SaveApplication(Application repoApplication)
        {
            if(repoApplication == null)
            {
                throw new ArgumentNullException("Application");
            }
            
            try
            {

                var appID = new SqlParameter("@appID", (repoApplication.AppId == 0) ? System.DBNull.Value : repoApplication.AppId);
                var appName = new SqlParameter("@appName", repoApplication.AppName);

                var outID = new SqlParameter();
                outID.ParameterName = "@outID";
                outID.SqlDbType = SqlDbType.Int;
                outID.Direction = ParameterDirection.Output;

                await _context.Database.ExecuteSqlRawAsync("EXEC usp_execApplication @appID = {0}," +
                     " @appName = {1}," +         
                     " @outID = {2} OUT ",
                     appID, appName,  outID);

                repoApplication.AppId = (int)outID.Value;

            }
            catch (SqlException oSqlException)   //stored procedure exeption
            {
                if (oSqlException.Number == Constants.ErrorCode.InvalidApplication)
                {
                    throw new DataValidationException(oSqlException.Message);
                }

                throw;  // any other exception
            }

        }


        public async Task   DeleteApplication (int id)
        {
            try
            {

                var appID = new SqlParameter("@appID", id);

                await _context.Database.ExecuteSqlRawAsync("EXEC usp_deleteApplication @appID = {0}",  id);


            }
            catch (SqlException oSqlException)
            {
                if (oSqlException.Number == Constants.ErrorCode.LinkedAppUsers || oSqlException.Number == Constants.ErrorCode.LinkedPermissions)
                {
                    throw new DataValidationException(oSqlException.Message);
                }

                throw;
            }
        }


        #endregion


    }
}
