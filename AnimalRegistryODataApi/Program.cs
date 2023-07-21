using AnimalRegistryODataApi.Configurations;
using Microsoft.AspNetCore.OData;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ConfigureLogging();

builder.Services.ConfigureDatabase(builder.Configuration);
builder.Services.ConfigureHealthChecks(builder.Configuration);
builder.Services.ConfigureRateLimiting(builder.Configuration);

builder.Services.ConfigureOData();

builder.Services.ConfigureCustomServices();

builder.Services.ConfigureAutoMapper();
builder.Services.ConfigureFluentValidation();

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCustomMiddleware();

// Configure the HTTP request pipeline.
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

app.MapControllers();

app.Run();