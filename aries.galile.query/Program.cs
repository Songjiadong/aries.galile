using aries.galile.query;
using Com.Ctrip.Framework.Apollo;

IConfiguration configuration = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json")
                            .Build();
var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureAppConfiguration((context, appBuilder) =>
{
    var apollo = configuration.GetSection("Apollo");
    var options = new ApolloOptions();
    configuration.Bind("Apollo", options);
    var apolloBuilder = appBuilder.AddApollo(options);
});

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
var service = builder.Services;
service.AddGrpc();



// Configure the HTTP request pipeline.

var app = builder.Build();
if (app.Environment.IsDevelopment())
{

}
// Configure the HTTP request pipeline.
app.MapGrpcService<QueryService>();
app.MapGet("/", () => "QueryServer is started.");
app.Run();
