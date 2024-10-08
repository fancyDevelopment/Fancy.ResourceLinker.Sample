using Fancy.ResourceLinker.Hateoas;
using FlightShopping.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddHateoas().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


string? connectionString = builder.Configuration.GetConnectionString("database");
builder.Services.AddDbContext<FlightShoppingDbContext>(options => options.UseSqlServer(connectionString));

IdentityModelEventSource.ShowPII = true;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.Authority = builder.Configuration.GetValue<string>("Authentication:Authority");
    options.Audience = builder.Configuration.GetValue<string>("Authentication:ClientId");
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters.NameClaimType = "preferred_username";
});

var app = builder.Build();

using var setupScope = app.Services.CreateScope();
setupScope.ServiceProvider.GetRequiredService<FlightShoppingDbContext>().Database.EnsureCreated();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
}

app.UseAuthorization();

app.MapControllers();

app.Run();
