using ClassLibrary.Application.Services;
using ClassLibrary.Core.Interfaces;
using ClassLibrary.Infrastructure.Data;
using ClassLibrary.Infrastructure.Repositories;
using gRPC.HunterService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddGrpc();

builder.Services.AddScoped<OracleDbContext>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("OracleDb");
    var logger = sp.GetRequiredService<ILogger<OracleDbContext>>();
    return new OracleDbContext(connectionString, logger);
});

builder.Services.AddScoped<IHunterRepository, HunterRepository>();
builder.Services.AddScoped<IHunterService, HunterService>();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
app.MapGrpcService<HunterGrpcService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
