using Winton.Extensions.Configuration.Consul;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Configuration.AddConsul(
    key: "config/myapp",
    options =>
    {
        options.ConsulConfigurationOptions = cco =>
        {
            cco.Address = new Uri("http://localhost:8500");
        };
        options.ReloadOnChange = true;
        options.Optional = true;
    });

var app = builder.Build();


app.MapGet("/", (IConfiguration config) =>
{
    var greeting = config["greeting"];
    var timeout = config["timeout"];
    return $"Greeting: {greeting}, Timeout: {timeout}";
});
app.Run();