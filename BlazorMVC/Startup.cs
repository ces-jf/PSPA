using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Blazored.Modal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BlazorSite.Data;
using System.IO;
using Microsoft.AspNetCore.Http;
using SystemHelper;
using SystemHelper.ViewModel;
using SystemHelper.Configurations;
using System.Net.Http;
using Infra.Interfaces;
using Microsoft.AspNetCore.Identity;
using Infra.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace BlazorSite
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //Default
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            Configuration = builder.Build();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddRestIndentity(Configuration);
            services.AddDependencyInjection();
            services.AddBlazoredModal();
            services.AddOptions();

            services.Configure<Configuration>(Configuration.GetSection("GeneralConfigurations"));
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.Configure<ElasticSearch>(Configuration.GetSection("ElasticSearch"));

            services.AddScoped<ToastService>();

            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddHttpContextAccessor();
            services.AddScoped<HttpContextAccessor>();
            services.AddHttpClient();
            services.AddScoped<HttpClient>();

            // If using Kestrel:
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            // If using IIS:
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IIdentityContext identityContext, RoleManager<IdentityRole> _roleManager, UserManager<Usuario> _userManager)
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

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCookiePolicy();
            app.UseAuthentication();

            ConfigureDataBase(identityContext, _roleManager, _userManager);
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
                endpoints.MapDefaultControllerRoute();
            });
        }

        private static void ConfigureDataBase(IIdentityContext identityContext, RoleManager<IdentityRole> _roleManager, UserManager<Usuario> _userManager)
        {
            identityContext.Database.Migrate();

            if (!_roleManager.RoleExistsAsync("Administrator").GetAwaiter().GetResult())
            {
                var role = new IdentityRole() { Name = "Administrator" };
                _roleManager.CreateAsync(role).GetAwaiter().GetResult();
            }

            if (!_roleManager.RoleExistsAsync("Manager").GetAwaiter().GetResult())
            {
                var role = new IdentityRole() { Name = "Manager" };
                _roleManager.CreateAsync(role).GetAwaiter().GetResult();
            }

            if (!_roleManager.RoleExistsAsync("Queryable").GetAwaiter().GetResult())
            {
                var role = new IdentityRole() { Name = "Queryable" };
                _roleManager.CreateAsync(role).GetAwaiter().GetResult();
            }

            var users = _userManager.GetUsersInRoleAsync("Administrator").GetAwaiter().GetResult();

            if (users.Count > 0)
                return;

            var user = new Usuario
            {
                UserName = "Administrator",
                Email = "admin@admin.com",
                FirstName = "Administrator",
                SecondName = "Administrator"
            };

            _userManager.CreateAsync(user, "@Dm1nistrator").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(user, "Administrator").Wait();
        }
    }
}
