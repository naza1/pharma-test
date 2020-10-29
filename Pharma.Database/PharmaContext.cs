using Microsoft.EntityFrameworkCore;
using Pharma.Common.Model;
using Pharma.Database.Mappings;

namespace Pharma.Database
{
    public class PharmaContext : DbContext
    {
        public PharmaContext(DbContextOptions<PharmaContext> options) : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("public");

            modelBuilder.ApplyConfiguration(new AccountConfiguration());
        }
    }
}
