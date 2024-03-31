using AnimalRegistryODataApi.Configurations;
using AnimalRegistryODataApi.Middleware;
using Application.AutoMapperProfiles;
using Microsoft.AspNetCore.OData;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ConfigureLogging();

builder.Services.ConfigureDatabase(builder.Configuration);
builder.Services.ConfigureHealthChecks(builder.Configuration);
builder.Services.ConfigureRateLimiting(builder.Configuration);

builder.Services.ConfigureOData(builder.Configuration);
builder.Services.AddResponseCaching();

builder.Services.ConfigureServices();

builder.Services.AddAutoMapper(typeof(OwnerProfile).Assembly);
builder.Services.ConfigureFluentValidation();

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHealthChecks();

app.UseRateLimiter();

app.UseODataBatching();
app.UseODataRouteDebug();
app.UseRouting();

app.UseResponseCaching();

app.MapControllers();

app.Run();