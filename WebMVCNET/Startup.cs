﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using SystemHelper;
using Infra.Interfaces;
using SystemHelper.ViewModel;
using Microsoft.AspNetCore.Identity;
using Infra.Entidades;
using SystemHelper.Configurations;

namespace WebMVCNET
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
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
            services.AddOptions();

            services.Configure<Configuration>(Configuration.GetSection("GeneralConfigurations"));
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.Configure<ElasticSearch>(Configuration.GetSection("ElasticSearch"));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IIdentityContext identityContext, RoleManager<IdentityRole> _roleManager, UserManager<Usuario> _userManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            ConfigureDataBase(identityContext, _roleManager, _userManager);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
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

            _userManager.CreateAsync(user, "administrator").GetAwaiter().GetResult();
        }
    }
}
