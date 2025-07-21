using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;

namespace Growy.Function;
public class CustomOpenApiConfigurationOptions : DefaultOpenApiConfigurationOptions
{
    public override OpenApiInfo Info => new()
    {
        Title = "Growy App",
        Version = "v1.0.0",
        Description = "Swagger UI for Growy App Backend Function",
        Contact = new OpenApiContact
        {
            Name = "Jiajin Zheng",
            Email = "jackytsheng@gmail.com",
        }
    };

    public override OpenApiVersionType OpenApiVersion { get; set; } = OpenApiVersionType.V3;
    public override bool ForceHttps => Environment.GetEnvironmentVariable("FUNCTIONS_ENVIRONMENT") != "Development";
    public override List<OpenApiServer> Servers => new()
    {
        new () { Url = "http://localhost:7020/api" ,Description = "Local"},
        new () { Url = "https://growyfunction.azurewebsites.net/api" , Description = "Production"}
    };
}