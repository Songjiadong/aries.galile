using aries.collector.manage;
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

var service = builder.Services;
service.AddGrpc();


var app = builder.Build();
if (app.Environment.IsDevelopment())
{

}
// Configure the HTTP request pipeline.
app.MapGrpcService<CollectService>();
app.MapGet("/", () => "CollectServer is started.");
app.Run();
