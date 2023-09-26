using FlightManagement.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Fancy.ResourceLinker.Hateoas;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddHateoas();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string? connectionString = builder.Configuration.GetConnectionString("database");
builder.Services.AddDbContext<FlightManagementDbContext>(options => options.UseSqlServer(connectionString));

IdentityModelEventSource.ShowPII = true;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.Authority = builder.Configuration.GetValue<string>("Authentication:Authority");
    options.Audience = builder.Configuration.GetValue<string>("Authentication:Audience");
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters.NameClaimType = "preferred_username";
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<FlightManagementDbContext>().Database.EnsureCreated();
}

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
