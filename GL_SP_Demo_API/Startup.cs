using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using GL_SP_Demo_BLL.Route;
using GL_SP_Demo_DAL.Route;
using GL_SP_Demo_Graph;
using GL_SP_Demo_Graph.Utilities;

namespace GL_SP_Demo_API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHaversine, Haversine>();
            services.AddSingleton<IGL_Graph, GL_SP_Route_Graph>();
            services.AddScoped<IGL_DAL, GL_SP_Route_DAL>();

            services = Set_RouteBLL_Configuration(services);            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);           
        }

        private IServiceCollection Set_RouteBLL_Configuration(IServiceCollection services)
        {
            services.AddScoped<IGL_Route_BLL, GL_SP_Route_BLL>();
            services.AddScoped<IRouteMessages, Messages_English>();
            services.AddScoped<IRouteMessageHandler, GL_SP_Route_MessageHandler>();
            services.AddScoped<IRouteTransformationStore, RouteTransformationStore>();
            services.AddScoped<IRouteTransformationHandler, RouteTransformationHandler>();
            services.AddScoped<IRouteValidatorStore, RouteValidatorStore>();
            services.AddScoped<IRouteValidationHandler, RouteValidationHandler>();
            return services;
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
