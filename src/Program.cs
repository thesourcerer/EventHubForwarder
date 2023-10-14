using EventHubForwarder;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var eventHubForwarderSettings = configuration.GetSection("EventHubForwarderSettings").Get<EventHubForwarderSettings>();

builder.WebHost.UseUrls("http://0.0.0.0:"+ eventHubForwarderSettings.WebServerPort);

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

EventHubProducer.InitProducer(eventHubForwarderSettings.EventHubSenderConnectionString);

app.Run();
