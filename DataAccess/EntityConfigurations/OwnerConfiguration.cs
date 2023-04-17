using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.EntityConfigurations;

public class OwnerConfiguration : IEntityTypeConfiguration<Owner>
{
    public void Configure(EntityTypeBuilder<Owner> builder)
    {
        builder.HasKey(o  => o.Id);

        builder.HasMany(o => o.Animals)
            .WithOne(a => a.Owner);

        builder.Property(o => o.FirstName)
            .HasMaxLength(25);

        builder.Property(o => o.LastName)
            .HasMaxLength(25);

        builder.Property(o => o.Age)
            .HasColumnType("tinyint unsigned");

        builder.Property(o => o.Email)
            .HasMaxLength(50);

        builder.Property(o => o.PhoneNumber)
            .HasMaxLength(20);

        builder.ToTable("Owners");
    }
}
