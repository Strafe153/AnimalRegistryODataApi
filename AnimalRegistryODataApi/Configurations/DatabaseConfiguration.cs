using Domain.Constants;
using Domain.Entities;
using Domain.Interfaces;
using DataAccess.EntityMappings;
using DataAccess.MapperSessions;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;

namespace AnimalRegistryODataApi.Configurations;

public static class DatabaseConfiguration
{
	public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
	{
		var mapper = new ModelMapper();
		mapper.AddMappings(typeof(AnimalMapping).Assembly.ExportedTypes);

		var domainMapping = mapper.CompileMappingForAllExplicitlyAddedEntities();

		var cfg = new Configuration().DataBaseIntegration(options =>
		{
			options.Driver<OracleManagedDataClientDriver>();
			options.Dialect<Oracle12cDialect>();
			options.ConnectionString = configuration.GetConnectionString(ConnectionStringConstants.DefaultConnection);
			options.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
			options.SchemaAction = SchemaAutoAction.Validate;
			options.LogFormattedSql = true;
			options.LogSqlInConsole = true;
		});

		cfg.AddMapping(domainMapping);

		new SchemaExport(cfg).Create(false, false);

		var sessionFactory = cfg.BuildSessionFactory();

		services.AddSingleton(sessionFactory);
		services.AddScoped(f => sessionFactory.OpenSession());
		services.AddScoped<IMapperSession<Owner>, OwnerMapperSession>();
		services.AddScoped<IMapperSession<Animal>, AnimalMapperSession>();
	}
}
