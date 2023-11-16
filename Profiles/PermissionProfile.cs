using AutoMapper;
using ODataSample.Data;
using ODataSample.Models;

namespace ODataSample.Profiles
{
    public class PermissionProfile :Profile
    {
        public PermissionProfile()
        {


            this.CreateMap<AppPermission, PermissionModel>()
                .ForMember(c => c.ApplicationID, o => o.MapFrom(c => c.AppId))
                .ForMember(c => c.PermissionID, o => o.MapFrom(c => c.ApId))
                .ForMember(c => c.Permission, o => o.MapFrom(c => c.PermName))
                .ForMember(c => c.IsActive, o => o.MapFrom(c => c.ApActive))
                //...
                  .ReverseMap();

        }

    }
}
