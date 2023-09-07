using Fancy.ResourceLinker.Gateway;
using Fancy.ResourceLinker.Models.Json;
using Fancy.ResourceLinker.Hateoas;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Logging;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("./settings/general.json", true);
builder.Configuration.AddJsonFile("./settings/gateways.adminapi.json", true);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.AddResourceConverter();
});

builder.Services.AddGateway()
                .LoadConfiguration(builder.Configuration.GetSection("Gateway"))
                .AddRouting()
                .AddAntiForgery()
                .AddAuthentication();

builder.Services.AddHateoas();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

IdentityModelEventSource.ShowPII = true;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:4200"));
}

app.UseGatewayAuthentication();
app.UseGatewayAuthenticationEndpoints();
//app.UseGatewayAntiForgery();

app.MapControllers();

app.UseGatewayRouting();

app.Run();
