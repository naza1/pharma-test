using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pharma.Common.Model;

namespace Pharma.Database.Mappings
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.Property(x => x.Username)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(x => x.FirstName)
                .HasMaxLength(255);

            builder.Property(x => x.LastName)
                .HasMaxLength(255);

            builder.Property(x => x.LastName)
                .HasMaxLength(255);

            builder.Property(x => x.PasswordHash)
                .HasMaxLength(255);

            builder.Property(x => x.PasswordSalt)
                .HasMaxLength(255);
        }
    }
}
