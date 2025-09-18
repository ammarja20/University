using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using University.Entities;

public class UserConfiguration : IEntityTypeConfiguration<Users>
{
    public void Configure(EntityTypeBuilder<Users> e)
    {
        e.ToTable("Users");
        e.HasKey(x => x.UserId);

        e.Property(x => x.UserName).HasMaxLength(64).IsRequired();
        e.Property(x => x.FirstName).HasMaxLength(64).IsRequired();
        e.Property(x => x.LastName).HasMaxLength(64).IsRequired();
        e.Property(x => x.EmailAddress).HasMaxLength(128).IsRequired();
        e.Property(x => x.PhoneNumber).HasMaxLength(16).IsRequired();
        e.Property(x => x.Role).HasMaxLength(32).IsRequired();

        // Unique email
        e.HasIndex(x => x.EmailAddress).IsUnique();
    }
}
