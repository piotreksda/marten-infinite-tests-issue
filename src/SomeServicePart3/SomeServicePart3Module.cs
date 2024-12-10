using Marten;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace SomeServicePart3;

public static class SomeServicePart3Module
{
    public static IServiceCollection AddSomeServicePart3Services(this IServiceCollection services,
        MartenServiceCollectionExtensions.MartenConfigurationExpression martenConfiguration)
    {
        return services;
    }

    public static IEndpointRouteBuilder MapSomeServicePart3Endpoints(this IEndpointRouteBuilder endpoints)
    {
        return endpoints;
    }
}