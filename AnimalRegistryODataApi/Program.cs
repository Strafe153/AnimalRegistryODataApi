using Application.Services;
using Core.Entities;
using Core.Interfaces;
using DataAccess;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AnimalRegistryContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var modelBuilder = new ODataConventionModelBuilder();
modelBuilder.EntitySet<Owner>("Owners");
modelBuilder.EntitySet<Animal>("Animals");

builder.Services
    .AddControllers()
    .AddOData(options =>
        options
            .EnableQueryFeatures(null)
            .AddRouteComponents("odata", modelBuilder.GetEdmModel()));

builder.Services.AddScoped<IService<Owner, Guid>, OwnersService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapControllers();

app.Run();
