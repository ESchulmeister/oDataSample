using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Linq;
using ODataSample.Data;
using ODataSample.Services;
using ODataSample.Utilities;

namespace ODataSample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ODataSample", Version = "v1" });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });

            //log4net init
            services.Configure<ConsoleLifetimeOptions>(opts => opts.SuppressStatusMessages = true);

            services.AddMvc(options => options.EnableEndpointRouting = false)
                 .AddNewtonsoftJson()
                .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

            //Database connection(s)
            services.AddDbContext<TodoContext>(opt => opt.UseInMemoryDatabase("TodoList"));

            services.AddDbContext<SampleContext>(
                     options => options.UseSqlServer("name=ConnectionStrings:UserDB"));

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


           services.AddScoped<IRepository, Repository>();
           services.AddScoped<IApplicationRepository, ApplicationRepository>();

            ////CORS  - enable cross-origin http requests
            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });


            //Register oData services  - Enable  OData operations
            services.AddControllers()
                .AddOData(opt =>
                {
                    opt.AddRouteComponents("odata_sample", EdmModelBuilder.GetSampleEdmModel());
                }
            );

            services.AddControllers()
                 // .AddOData(opt => opt.EnableQueryFeatures()
                 //.AddOData(opt => opt.Expand()
                 .AddOData(o =>
                {
                    o.AddRouteComponents("odata", EdmModelBuilder.GetEdmModel());
                    o.Select();
                    o.Filter();
                    o.Expand();
                    o.OrderBy();
                    o.Count();

                });
              //  .AddRouteComponents("odata", EdmModelBuilder.GetEdmModel()));


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ODataSample  v1"));
                app.UseDeveloperExceptionPage();

            }
            else
            {
                app.UseExceptionHandler("/error");
                app.UseHsts();
            }


            app.UseHttpsRedirection();
            app.UseMiddleware(typeof(ErrorHandler));


            // Use odata route debug, /$odata
            app.UseODataRouteDebug();


            // Add OData /$query middleware
            app.UseODataQueryRequest();

            // Add the OData Batch middleware to support OData $Batch
            app.UseODataBatching();

            app.UseRouting();
            app.UseAuthorization();
            app.UseCors("CorsPolicy");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });



        }




    }
}
