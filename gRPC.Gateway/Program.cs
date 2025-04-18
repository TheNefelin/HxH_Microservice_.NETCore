using gRPC.HunterNenService;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// gRPC + Transcoding
builder.Services.AddGrpc().AddJsonTranscoding();

// gRPC Client
builder.Services.AddGrpcClient<HunterNenProto.HunterNenProtoClient>(o =>
{
    o.Address = new Uri("https://localhost:7108");
});

// Add Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "gRPC Gateway API", Version = "v1" });
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.MapDefaultEndpoints();

// ✅ Use Swagger
app.UseSwagger();
app.UseSwaggerUI();

// ✅ Enable CORS
app.UseCors();

// Add Controllers
app.MapControllers();
app.MapGet("/", () => "gRPC Gateway running");

app.Run();
