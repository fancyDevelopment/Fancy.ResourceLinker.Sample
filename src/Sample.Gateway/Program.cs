using Fancy.ResourceLinker.Gateway;
using Fancy.ResourceLinker.Hateoas;
using Microsoft.IdentityModel.Logging;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddHateoas();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddGateway()
                .LoadConfiguration(builder.Configuration.GetSection("Gateway"))
                .AddRouting()
                .AddAntiForgery()
                .AddAuthentication();

builder.Services.AddOpenTelemetry()
                .WithTracing(builder =>
                {
                    builder.AddSource("Sample.Gateway")
                           .ConfigureResource(resource => resource.AddService("Sample.Gateway"))
                           .AddAspNetCoreInstrumentation()
                           .AddHttpClientInstrumentation()
                           .AddOtlpExporter(opts => opts.Endpoint = new Uri("http://localhost:4317"));
                });

IdentityModelEventSource.ShowPII = true;

var app = builder.Build();

app.UseGatewayAuthentication();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
}

app.UseGatewayAuthenticationEndpoints();
app.UseGatewayAntiForgery();

app.MapControllers();

app.UseGatewayRouting();

app.Run();
