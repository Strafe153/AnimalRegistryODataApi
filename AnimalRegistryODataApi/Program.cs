using AnimalRegistryODataApi.Configurations;
using AnimalRegistryODataApi.Middleware;
using Application.Validators.Owner;
using FluentValidation;
using Microsoft.AspNetCore.OData;

var builder = WebApplication.CreateBuilder(args);

builder.Logging
	.ClearProviders()
	.AddLog4Net("log4net.config");

builder.Services.ConfigureDatabase(builder.Configuration);
builder.Services.ConfigureHealthChecks(builder.Configuration);
builder.Services.ConfigureRateLimiting(builder.Configuration);

builder.Services.ConfigureOData(builder.Configuration);
builder.Services.AddResponseCaching();

builder.Services.ConfigureServices();
builder.Services.AddSingleton<ExceptionHandlingMiddleware>();

builder.Services.AddValidatorsFromAssemblyContaining<OwnerCreateDtoValidator>();

builder.Services.ConfigureSwagger();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHealthChecks();

app.UseODataBatching();
app.UseODataRouteDebug();
app.UseRouting();

app.UseRateLimiter();

app.UseResponseCaching();

app.MapControllers();

app.Run();