using Microsoft.EntityFrameworkCore;

namespace RC.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<Agent> Agents { get; set; }
        public DbSet<AgentResponse> AgentResponses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=rcdb;Username=postgres;Password=XXXX");
        }
    }
}
