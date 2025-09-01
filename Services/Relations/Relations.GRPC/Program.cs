using Relations.GRPC.Services;
using Relations.Common.Repositories;
using Relations.Common.Data;
using Common.Logger.Extensions;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddScoped<IRelationsContext, RelationsContext>();
builder.Services.AddScoped<IRelationsRepository, RelationsRepository>();

// Add logger service
builder.Services.AddLogger(options =>
{
    options.LogDirectory = "/app/logs/relations-grpc";
    options.EnableFileLogging = true;
});

builder.Services.AddGrpc();

var app = builder.Build();

// Add logger middleware early in the pipeline
app.UseLogger();

// Configure the HTTP request pipeline.
app.MapGrpcService<RelationsService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
