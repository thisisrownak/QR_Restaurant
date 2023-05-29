using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR_Restaurant.DAL.Context
{
    public class ContextFactory : IDesignTimeDbContextFactory<QR_Context>
    {
        public QR_Context CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<QR_Context>();
            string projectPath = AppDomain.CurrentDomain.BaseDirectory.Split(new String[] { @"bin\" }, StringSplitOptions.None)[0];
            IConfigurationRoot configuration = new ConfigurationBuilder()
             .SetBasePath(projectPath)
             .AddJsonFile("appsettings.json")
             .Build();

            builder.UseSqlServer(configuration.GetConnectionString("MssqlConnection"));
            return new QR_Context(builder.Options);
        }
    }
}
