using Api.Modules;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoWrapper;
using Data.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// replace default DI with Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule<RepositoryModule>();
    containerBuilder.RegisterModule<ServiceModule>();
});

// Add controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); // Swagger needs this
builder.Services.AddSwaggerGen();           // Swagger generator

builder.Services.AddDbContext<UniversityDbContext>(opts =>
    opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Enable Swagger UI in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseApiResponseAndExceptionWrapper(); // AutoWrapper
app.MapControllers();

app.Run();