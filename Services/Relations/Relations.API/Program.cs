using EventBusMessages.Constants;
using MassTransit;
using Relations.API.EventBsConsumers;
using Relations.Common.Extensions;
using Common.Logger.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<UserCreatedConsumer>();
    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(builder.Configuration.GetValue<string>("EventBusSettings:HostAddress"));
        cfg.ReceiveEndpoint(EventBusConstants.UserCreatedReplicationToRelationServiceQueue, c =>
        {
            c.ConfigureConsumer<UserCreatedConsumer>(ctx);
        });
    });
});

// Add simple logger middleware
builder.Services.AddLogger(options =>
{
    options.LogBodies = true;
    options.LogHeaders = true;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRelationsCommonServices();
builder.Services.AddControllers();

var app = builder.Build();

// Add logger middleware early in the pipeline
app.UseLogger();

app.UseCors("CorsPolicy");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
