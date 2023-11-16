using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using ODataSample.Data;
using ODataSample.Models;

namespace ODataSample.Utilities
{
    public class EdmModelBuilder
    {

        //private static IEdmModel _edmModel;

    //    public static IEdmModel EdmModel { get => _edmModel; set => _edmModel = value; }


        // map the entityset which is the type returned
        // from the endpoint onto the OData pipeline
        // return fully configured builder model 
        // used for building  OData library

        public static IEdmModel GetSampleEdmModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<TodoItem>("items");
            return builder.GetEdmModel();
        }

        public static IEdmModel GetEdmModel()
        {
            var modelBuilder = new ODataConventionModelBuilder();

            modelBuilder.EntitySet<ApplicationModel>("applications");

            //ComplexTypeConfiguration<PermissionModel> permissions = modelBuilder.ComplexType<PermissionModel>();
            //permissions.Property(c => c.ApplicationID);

           modelBuilder.ComplexType<PermissionModel>();

          //  modelBuilder.EntityType<PermissionModel>().HasKey(e => e.ApplicationID);

           return modelBuilder.GetEdmModel();


        }
    }



    }
