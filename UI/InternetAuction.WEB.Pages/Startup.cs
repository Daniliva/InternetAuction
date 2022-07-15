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
using InternetAuction.BLL.Contract;
using InternetAuction.BLL;
using InternetAuction.BLL.DTO;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace InternetAuction.WEB.Pages
{
    /// <summary>
    /// The startup.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Configures the services.
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">The services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            IFactory factory = new ServiceFactory(
                  new UnitOfWorkMSSQL(new string[]
                  {
                    Configuration.GetConnectionString("MSSqlConnectionString"),
                    Configuration.GetConnectionString("MongoConnectionString")
                  }));
            services.AddSingleton<IExpansionGetEmail<UserModel, string>>(factory.Get<IExpansionGetEmail<UserModel, string>>());
            services.AddSingleton<ICrud<RoleUserModel, string>>(factory.Get<ICrud<RoleUserModel, string>>());
            services.AddSingleton<ICrud<RoleModel, string>>(factory.Get<ICrud<RoleModel, string>>());

            services.AddSingleton<ICrud<LotModel, int>>(factory.Get<ICrud<LotModel, int>>());

            services.AddSingleton<ICrud<LotCategoryModel, int>>(factory.Get<ICrud<LotCategoryModel, int>>());
            services.AddSingleton<ICrud<AutctionStatusModel, int>>(factory.Get<ICrud<AutctionStatusModel, int>>());

            services.AddSingleton<ICrud<AutctionModel, int>>(factory.Get<ICrud<AutctionModel, int>>());
            services.AddSingleton<ICrud<BiddingModel, int>>(factory.Get<ICrud<BiddingModel, int>>());

            services.AddControllersWithViews();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/LogIn");
                    options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Account/LogIn");
                });
        }

        /// <summary>
        /// Configures the.
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The app.</param>
        /// <param name="env">The env.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            /*   if (env.IsDevelopment())
               {
                   app.UseDeveloperExceptionPage();
               }
               else
               {
                   app.UseExceptionHandler("/Home/Error");
                   // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                   app.UseHsts();
               }*/
            app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
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