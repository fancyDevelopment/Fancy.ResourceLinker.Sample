using Fancy.ResourceLinker.Hateoas;
using Fancy.ResourceLinker.Models.Json;
using FlightShopping.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("./settings/common.json", true);
builder.Configuration.AddJsonFile("./settings/flightshopping.json", true);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.AddResourceConverter(true);
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHateoas();

string? connectionString = builder.Configuration.GetConnectionString("database");
builder.Services.AddDbContext<FlightShoppingDbContext>(options => options.UseSqlServer(connectionString));

var app = builder.Build();

using var setupScope = app.Services.CreateScope();
setupScope.ServiceProvider.GetRequiredService<FlightShoppingDbContext>().Database.EnsureCreated();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
