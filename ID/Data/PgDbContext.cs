using ID.Domain.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ID.Data
{
    public class PgDbContext : IdentityDbContext<User>
    {
        public PgDbContext(DbContextOptions<PgDbContext> options) : base(options) { }

        /*public DbSet<Entity> Entities { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
            base.OnModelCreating(modelBuilder);
        }*/
    }
}
