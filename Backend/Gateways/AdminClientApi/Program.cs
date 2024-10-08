using Fancy.ResourceLinker.Gateway;
using Fancy.ResourceLinker.Hateoas;
using Microsoft.IdentityModel.Logging;

IdentityModelEventSource.ShowPII = true;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddHateoas();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddGateway()
                .LoadConfiguration(builder.Configuration.GetSection("Gateway"))
                .AddRouting()
                .AddAuthentication()
                .AddAntiForgery();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
}

app.UseGatewayAuthentication();
app.UseGatewayAuthenticationEndpoints();
app.UseGatewayAntiForgery();

app.MapControllers();

app.UseGatewayRouting();

app.Run();
