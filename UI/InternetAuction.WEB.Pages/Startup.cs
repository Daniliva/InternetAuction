using InternetAuction.DAL.Contract;
using InternetAuction.DAL.MSSQL;
using InternetAuction.DAL.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;

namespace InternetAuction.WEB.Pages
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
            Configuration.GetConnectionString("MSSqlConnectionString");
            string abc = Configuration.GetConnectionString("MSSqlConnectionString");

            //  var temp = ConfigurationManager<string>();    //ConnectionStrings["MSSqlConnectionString"].ConnectionString;
            var temp2 = Configuration.GetConnectionString("MongoConnectionString");

            //      ConfigurationManager.ConnectionStrings["MongoConnectionString"].ConnectionString;
            services.AddControllersWithViews();
            services.AddSingleton<IUnitOfWorkMSSQL>(new UnitOfWorkMSSQL(new string[] { abc, temp2 }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}