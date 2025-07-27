using EventBusMessages.Constants;
using MassTransit;
using Relations.API.EventBsConsumers;
using Relations.Common.Extensions;

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

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRelationsCommonServices();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
