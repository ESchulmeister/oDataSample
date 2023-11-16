using AutoMapper;
using ODataSample.Models;
using ODataSample.Data;

namespace ODataSample
{
   
    /// <summary>
    /// Data mapping between 
    /// Application Entity - Data/Application.cs - dbo.Application table  
    /// AND
    /// Applicatation Model - /Models/ApplicationModel.cs  
    /// </summary>
    public class ApplicationProfile :Profile
    {

        public ApplicationProfile()
        {

            this.CreateMap<Application, ApplicationModel>()
                .ForMember(c => c.ID, o => o.MapFrom(c => c.AppId))
                .ForMember(c => c.Name, o => o.MapFrom(c => c.AppName))
                .ForMember(c => c.IsActive, o => o.MapFrom(c => c.AppActive))
                .ForMember(c => c.CreatedBy, o => o.MapFrom(c => c.AppCreatedBy))
                .ForMember(c => c.CreatedDate, o => o.MapFrom(c => c.AppCreatedDate))
                .ForMember(c => c.CreatedBy, o => o.MapFrom(c => c.AppModifiedBy))
                .ForMember(c => c.ModifiedDate, o => o.MapFrom(c => c.AppModifiedDate))

                .ForMember(c => c.Permissions, o => o.MapFrom(c => c.AppPermissions))

                .ReverseMap();

        }
    }
}
