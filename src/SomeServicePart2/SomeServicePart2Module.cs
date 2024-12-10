using Marten;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace SomeServicePart2;

public static class SomeServicePart2Module
{
    public static IServiceCollection AddSomeServicePart2Services(this IServiceCollection services,
        MartenServiceCollectionExtensions.MartenConfigurationExpression martenConfiguration)
    {
        return services;
    }

    public static IEndpointRouteBuilder MapSomeServicePart2Endpoints(this IEndpointRouteBuilder endpoints)
    {
        return endpoints;
    }
}