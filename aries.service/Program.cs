using aries.common.swagger;
using Com.Ctrip.Framework.Apollo;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Text.Encodings.Web;
using System.Text.Unicode;

IConfiguration configuration = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json")
                            .Build();
var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureAppConfiguration((context, appBuilder) =>
{

    var apollo = configuration.GetSection("Apollo");
    appBuilder.AddApollo(apollo).AddDefault();

});
// Add services to the container.
var services = builder.Services;
services.AddControllers().AddJsonOptions(options => {
    options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
    options.JsonSerializerOptions.IgnoreReadOnlyProperties = false;
    options.JsonSerializerOptions.IgnoreReadOnlyFields = false;
    options.JsonSerializerOptions.WriteIndented = true;
    options.JsonSerializerOptions.IncludeFields = true;

});
services.AddDaprClient();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(c =>
{
    typeof(ApiVersion).GetEnumNames().ToList().ForEach(version =>
    {
        c.SwaggerDoc(version, new OpenApiInfo()
        {
            Title = $"Aries Service RESTful API {version}",
            Version = version,
            Description = $"AriesService {version}",
            Contact = new OpenApiContact() { Email = "songjiadong@miic.com.cn", Name = "JustinSong", Url = new Uri("http://www.miic.com.cn") }
        });
        c.AddServer(new OpenApiServer() { Url = "http://" + Environment.GetEnvironmentVariable("HOST_IP"), Description = Environment.GetEnvironmentVariable("HOST_IP") });
        c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

    });
    c.OrderActionsBy(o => o.RelativePath);
     //Directory.GetFiles(AppContext.BaseDirectory, CommonSource.CommonXmlConfig["appSettings:add:WebApiMetaDataFile:value"].ToString(), SearchOption.AllDirectories).ToList().ForEach(f => options.IncludeXmlComments(f, true));
    c.DocumentFilter<ApiTagFilter>();
    c.OperationFilter<FileUploadFilter>();

});
var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        typeof(ApiVersion).GetEnumNames().ToList().ForEach(version =>
        {
            options.SwaggerEndpoint(url: $"/swagger/{version}/swagger.json", name: $"Aries Service RESTful API {version}");
        });

        options.DocExpansion(DocExpansion.None);
        options.DefaultModelsExpandDepth(-1);
    });
}

app.UseAuthorization();
app.MapControllers();
app.Run();

