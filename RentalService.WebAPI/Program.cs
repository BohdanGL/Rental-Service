using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RentalService.Application.Infrastructure;
using RentalService.Application.Infrastructure.Mapping;
using RentalService.Persistence;
using RentalService.WebAPI.Infrastructure.Db;
using RentalService.WebAPI.Infrastructure.ExceptionsHandling.Extensions;
using RentalService.WebAPI.Infrastructure.Seeding;
using RentalService.WebAPI.Infrastructure.Swagger.Extensions;
using static RentalService.Application.Contracts.Queries.GetContracts;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGeneration();

builder.Services.AddDbContext<RentalServiceContext>(opt =>
  opt.UseSqlServer(configuration["Db:ConnectionString"]));

builder.Services.AddMediatR(typeof(Handler).Assembly);

// AutoMapper
var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehaviour<,>));
AssemblyScanner.FindValidatorsInAssembly(typeof(RequestValidationBehaviour<,>).Assembly)
    .ForEach(item => builder.Services.AddScoped(item.InterfaceType, item.ValidatorType));

var app = builder.Build();

await app.SetupDatabaseAsync();
await app.SeedDatabaseAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCustomExceptionHandler();

app.UseAuthorization();

app.MapControllers();

app.Run();
