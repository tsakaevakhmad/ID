using ID.Domain.Entity;
using ID.Extesions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;

namespace ID.Data
{
    public class PgDbContext : IdentityDbContext<User>
    {

        public PgDbContext(DbContextOptions<PgDbContext> options) : base(options) 
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Seeds(_manager);
            base.OnModelCreating(modelBuilder);
        }
    }
}
