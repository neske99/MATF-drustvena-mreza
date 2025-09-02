using Relations.GRPC.Services;
using Relations.Common.Repositories;
using Relations.Common.Data;
var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddScoped<IRelationsContext, RelationsContext>();
builder.Services.AddScoped<IRelationsRepository, RelationsRepository>();
builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<RelationsService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
