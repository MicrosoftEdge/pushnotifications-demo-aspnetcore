using Microsoft.EntityFrameworkCore;

namespace PushnotificationsDemo.Models
{
    public class DemoDbContext : DbContext
    {
        public DemoDbContext(DbContextOptions options) : base(options) { }
        
        public DbSet<PushSubscription> PushSubscription { get; set; }
        

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
