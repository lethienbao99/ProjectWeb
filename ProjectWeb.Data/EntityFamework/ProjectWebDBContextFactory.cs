using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Data.EntityFamework
{
    public class ProjectWebDBContextFactory : IDesignTimeDbContextFactory<ProjectWebDBContext>
    {
        public ProjectWebDBContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

            var connectionString = configuration.GetConnectionString("ProjectWebDB");
            var optionsBuilder = new DbContextOptionsBuilder<ProjectWebDBContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new ProjectWebDBContext(optionsBuilder.Options);
        }
    }
}
