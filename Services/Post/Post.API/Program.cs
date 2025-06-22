using EventBusMessages.Constants;
using MassTransit;
using Post.API.EventBsConsumers;
using Post.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.RegisterInfrastructureService(builder.Configuration);
builder.Services.AddMassTransit(config => {
    config.AddConsumer<UserCreatedConsumer>();
    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(builder.Configuration.GetValue<string>("EventBusSettings:HostAddress") );
        cfg.ReceiveEndpoint(EventBusConstants.UserCreatedQueue, c =>
        {
            c.ConfigureConsumer<UserCreatedConsumer>(ctx);
        });
    });
});
builder.Services.AddCors(options=>{
                options.AddPolicy("CorsPolicy",builder=>{
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                    });
                });


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("CorsPolicy");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

