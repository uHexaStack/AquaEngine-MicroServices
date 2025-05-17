using Consul;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

// 👉 Lógica para registrar microservicios en Consul KV al arrancar
using (var consulClient = new ConsulClient(config =>
       {
           config.Address = new Uri("http://localhost:8500");
       }))
{
    var services = new Dictionary<string, string>
    {
        ["gateway-service"] = "http://localhost:8000/"
  
    };

    foreach (var service in services)
    {
        var key = $"config/registry/{service.Key}/address";
        var value = Encoding.UTF8.GetBytes(service.Value);

        var result = await consulClient.KV.Put(new KVPair(key) { Value = value });

        Console.WriteLine($"Registered {service.Key} => {service.Value} | Success: {result.Response}");
    }
}

app.MapGet("/", () => "Config Service is running");
app.Run();