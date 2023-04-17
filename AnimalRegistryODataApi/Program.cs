using AnimalRegistryODataApi.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.ConfigureDatabase(builder.Configuration);
builder.Services.ConfigureOData();
builder.Services.ConfigureCustomServices();
builder.Services.ConfigureAutoMapper();
builder.Services.ConfigureFluentValidation();

var app = builder.Build();

app.MapControllers();

app.ApplyDatabaseMigration();
app.AddCustomMiddleware();

app.Run();