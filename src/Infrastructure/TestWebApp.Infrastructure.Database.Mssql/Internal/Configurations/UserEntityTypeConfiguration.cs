namespace TestWebApp.Infrastructure.Database.Mssql.Internal.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using TestWebApp.Domain;

    internal sealed class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Name).HasMaxLength(32).IsRequired();
            builder.Property(u => u.PasswordHash).HasMaxLength(48).IsRequired();
            builder.Property(u => u.Salt).HasMaxLength(32).IsRequired();
            builder.Property(u => u.CreatedAt).IsRequired();
        }
    }
}
