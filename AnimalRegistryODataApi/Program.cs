using AnimalRegistryODataApi.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureDatabase(builder.Configuration);
builder.Services.ConfigureHealthChecks(builder.Configuration);

builder.Services.ConfigureOData();

builder.Services.ConfigureCustomServices();

builder.Services.ConfigureAutoMapper();
builder.Services.ConfigureFluentValidation();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHealthChecks();

app.MapControllers();

app.ApplyDatabaseMigration();
app.AddCustomMiddleware();

app.Run();