namespace TestWebApp.Infrastructure.Database.Mssql.Internal.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using System;
    using TestWebApp.Domain;

    internal sealed class AccountEntityTypeConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("Accounts");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Name).HasMaxLength(32).IsRequired();
            builder.Property(a => a.Balance).IsRequired().HasPrecision(10, 2);
            builder.Property(a => a.IsActive).IsRequired();
            builder.HasOne(a => a.Owner).WithMany().HasForeignKey(a => a.Id);
        }
    }
}
