namespace TestWebApp.Infrastructure.Database.Mssql.Internal.Configurations
{
    using System;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using TestWebApp.Domain;

    internal class TransactionEntityTypeConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("Transactions");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Amount).IsRequired().HasPrecision(10, 2);
            builder.HasOne(t => t.From).WithMany().HasForeignKey(t => t.Id).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(t => t.To).WithMany().HasForeignKey(t => t.Id).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
