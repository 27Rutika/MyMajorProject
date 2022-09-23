using EInsurance.Data;
using EInsurance.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//ADD the assembly attribute
[assembly: ApiConventionType(typeof(DefaultApiConventions))]
namespace EInsurance
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
            services
              .AddDbContext<ApplicationDbContext>(options =>
              {
                  // Get the SQL Connection String from the AppSettings.json file
                  string connString = Configuration.GetConnectionString("MyDefaultConnectionString");

                  options.UseSqlServer(connString);
              });
            //Register the OWIN Identity Middleware
            services
              .AddIdentity<IdentityUser, IdentityRole>(options =>
              {
                  options.SignIn.RequireConfirmedAccount = true;
                  options.Password.RequiredLength = 8;
              })
              .AddEntityFrameworkStores<ApplicationDbContext>()
              .AddDefaultTokenProviders();

            //changes
            // Register the ASP.NET Razor Pages Middleware
            services
                    .AddRazorPages()
                    .AddRazorPagesOptions(options =>
                       {
                           options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");
                           options.Conventions.AuthorizeAreaPage("Identity", "/Account/Logout");
                    });
            // Configure the Application Cookie options
            services
                .ConfigureApplicationCookie(options =>
                {
                    options.LoginPath = "/Identity/Account/Login";
                    options.LogoutPath = "/Identity/Account/Logout";
                    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);      // Default Session Cookie expiry is 20 minutes
                    options.SlidingExpiration = true;
                    options.Cookie.HttpOnly = true;
                    options.Cookie.Name = "MyAuthCookie";
                });
            
            //Register the MVC Middleware
            // Needed for swagger Documentation Middleware Services
            // Needed for API Support(if applicable)
            services.AddMvc();
            //Register the swagger deocumentation generation middleware service
            services
                .AddSwaggerGen(config =>
                {
                    config.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "Insurance",
                        Description = "EInsurance - API Version 1.0"
                    });
                });

            //Register the EmailSender ot the dependency injection container
            services.AddSingleton<IEmailSender, MyEmailSenderService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            RoleManager<IdentityRole> rolemanager,
            UserManager<IdentityUser> usermanager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            // Add the Swagger Middleware
            app.UseSwagger();

            // Add the Swagger Documentation Generation Middleware
            // URL: https://localhost:xxxx/swagger
            app.UseSwaggerUI(config =>
            {
                config.SwaggerEndpoint("/swagger/v1/swagger.json", "Insurance Web API v1.0");
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            //Activate OWIN Middleware for Authentication and Authorization Services
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();

                // Register the ASP.NET Routes for Areas
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area}/{controller}/{action=Index}/{id?}");

                // Register the ASP.NET Routes for the MVC Controllers
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            ApplicationDbContextSeed.SeedIdentityRolesAsync(rolemanager).Wait();
            ApplicationDbContextSeed.SeedIdentityUserAsync(usermanager).Wait();
        }
    }
}
