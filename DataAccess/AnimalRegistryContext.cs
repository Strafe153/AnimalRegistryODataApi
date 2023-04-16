using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess;

public class AnimalRegistryContext : DbContext
{
	public DbSet<Owner> Owners => Set<Owner>();
	public DbSet<Animal> Animals => Set<Animal>();

	public AnimalRegistryContext(DbContextOptions<AnimalRegistryContext> options)
		: base(options)
	{
	}
}
