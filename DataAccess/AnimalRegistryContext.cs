using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class AnimalRegistryContext : DbContext
{
	public DbSet<Owner> Owners => Set<Owner>();
	public DbSet<Animal> Animals => Set<Animal>();

	public AnimalRegistryContext(DbContextOptions<AnimalRegistryContext> options)
		: base(options)
	{
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
	}
}
