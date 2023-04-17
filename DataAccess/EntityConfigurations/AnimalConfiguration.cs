using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.EntityConfigurations;

public class AnimalConfiguration : IEntityTypeConfiguration<Animal>
{
    public void Configure(EntityTypeBuilder<Animal> builder)
    {
        builder.HasKey(a => a.Id);

        builder.HasOne(a => a.Owner)
            .WithMany(o => o.Animals);

        builder.Property(a => a.PetName)
            .HasMaxLength(25);

        builder.Property(a => a.Kind)
            .HasMaxLength(50);

        builder.Property(o => o.Age)
            .HasColumnType("tinyint unsigned");

        builder.ToTable("Animals");
    }
}
