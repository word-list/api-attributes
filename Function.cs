namespace WordList.Api.Attributes;

using System.Text.Json.Serialization;
using Amazon.Lambda.APIGatewayEvents;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using WordList.Data.Sql;
using Microsoft.AspNetCore.Http;
using Amazon.Lambda.Serialization.SystemTextJson;

public class Function
{
    public static async Task Main(string[] args)
    {
        var attributes = (await WordAttributes.GetAllAsync().ConfigureAwait(false))
            .OrderBy(attr => attr.Name)
            .Select(attr => new AttributeDto
            {
                Name = attr.Name,
                Description = attr.Description,
                Min = attr.Min,
                Max = attr.Max,
                Display = attr.Display,
            })
            .ToArray();

        var app = CreateHostBuilder(args).Build();
        var api = app.MapGroup("/api");

        api.MapGet("/attributes", new RequestDelegate((context) =>
        {
            return context.Response.WriteAsJsonAsync(attributes, LambdaFunctionJsonSerializerContext.Default.AttributeDto);
        }));

        await app.RunAsync();
    }

    public static WebApplicationBuilder CreateHostBuilder(string[] args)
    {
        var builder = WebApplication.CreateSlimBuilder(args);

        builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi, new SourceGeneratorLambdaJsonSerializer<LambdaFunctionJsonSerializerContext>());

        builder.Services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.TypeInfoResolverChain.Insert(0, LambdaFunctionJsonSerializerContext.Default);
        });

        return builder;
    }
}

[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(AttributeDto))]
[JsonSerializable(typeof(AttributeDto[]))]
[JsonSerializable(typeof(APIGatewayHttpApiV2ProxyRequest))]
[JsonSerializable(typeof(APIGatewayHttpApiV2ProxyResponse))]
public partial class LambdaFunctionJsonSerializerContext : JsonSerializerContext
{
}