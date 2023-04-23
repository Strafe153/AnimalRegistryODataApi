using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataAccess;

public class AnimalRegistryContext : DbContext
{
	public virtual DbSet<Owner> Owners => Set<Owner>();
    public virtual DbSet<Animal> Animals => Set<Animal>();

	public AnimalRegistryContext(DbContextOptions<AnimalRegistryContext> options)
		: base(options)
	{
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
	}
}
