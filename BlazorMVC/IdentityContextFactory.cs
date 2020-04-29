using Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorSite
{
    public class IdentityContextFactory : IDesignTimeDbContextFactory<IdentityContext>
    {
        public IdentityContext CreateDbContext(string[] args)
        {
            var connectionString = $"Server=localhost;Database=PSPABase;User=root;Password=";
            var optionsBuilder = new DbContextOptionsBuilder<IdentityContext>();
            optionsBuilder.UseMySql(connectionString, mysqlOptions =>
            {
                mysqlOptions.ServerVersion(SystemHelper.Configuration.DatabaseVersion, ServerType.MySql);
                mysqlOptions.MigrationsAssembly("BlazorSite");
            });

            return new IdentityContext(optionsBuilder.Options);
        }
    }
}
