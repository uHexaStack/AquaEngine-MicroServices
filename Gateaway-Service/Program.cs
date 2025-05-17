using Consul;
using Ocelot.DependencyInjection;
using Ocelot.ServiceDiscovery;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.Provider.Polly;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("ocelot.json");

builder.Services.AddOcelot()
    .AddConsul()
    .AddPolly();
var app = builder.Build();

// Registrar en Consul
var consulClient = new ConsulClient();

var registration = new AgentServiceRegistration()
{
    ID = "gateway-service",
    Name = "gateway-service",
    Address = "localhost", // Si estás en Docker usa el nombre del contenedor
    Port = 8000
};

await consulClient.Agent.ServiceRegister(registration);

app.MapGet("/", () => "Gateway Service is running");

app.Lifetime.ApplicationStopping.Register(() =>
{
    consulClient.Agent.ServiceDeregister(registration.ID).Wait();
});

app.Run("http://localhost:8000");