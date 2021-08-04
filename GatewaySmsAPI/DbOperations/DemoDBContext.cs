using Microsoft.EntityFrameworkCore;

namespace GatewaySmsAPI.DbOperations
{
    public class DemoDBContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "DemoDB");
        }

        public DbSet<User> Users { get; set; }
    }
}