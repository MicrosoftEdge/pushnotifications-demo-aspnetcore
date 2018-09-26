using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;

namespace PushnotificationsDemo.Models
{
    public class DemoDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = Environment.GetEnvironmentVariable("Database", EnvironmentVariableTarget.Process);
            if (string.IsNullOrEmpty(connectionString)) throw new Exception("Application setting 'SqlDatabase' was not found. Add it in settings.json!");
            optionsBuilder.UseSqlServer(connectionString);
            var isDevelopment = Environment.GetEnvironmentVariable("Environment", EnvironmentVariableTarget.Process) == "Development";
            if (isDevelopment) optionsBuilder.UseLoggerFactory(new LoggerFactory(new[] { new DebugLoggerProvider() }));
        }

        public DbSet<PushSubscription> PushSubscription { get; set; }
        

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
