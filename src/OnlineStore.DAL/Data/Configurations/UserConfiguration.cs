using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.DAL.Entities;

namespace OnlineStore.DAL.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User");
        builder.Property(u => u.Login).HasMaxLength(50).IsRequired();
        builder.Property(u => u.PassHash).HasMaxLength(64).IsRequired();
        builder.HasIndex(u => u.Login).IsUnique();
    }
}
