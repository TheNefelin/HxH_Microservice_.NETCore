using gRPC.HunterService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddGrpc().AddJsonTranscoding();
//builder.Services.AddGrpcJsonTranscoding();

builder.Services.AddEndpointsApiExplorer(); // Swagger
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();       // Swagger UI
app.UseSwaggerUI();     // Swagger UI

// Configure the HTTP request pipeline.
app.MapGrpcService<HunterGrpcService>(); 

// ruta raíz útil para evitar errores en navegadores
//app.MapGet("/", () => "Este microservicio usa gRPC. Usa una herramienta gRPC o prueba con Swagger.");


app.Run();
