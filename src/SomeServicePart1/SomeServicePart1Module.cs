using Marten;
using Marten.Events.Projections;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using SomeServicePart1.Events;
using SomeServicePart1.Projections;
using SomeServicePart1.Subscriptions;

namespace SomeServicePart1;

public static class SomeServicePart1Module
{
    public static IServiceCollection AddSomeServicePart1Services(this IServiceCollection services,
        MartenServiceCollectionExtensions.MartenConfigurationExpression martenConfiguration)
    {
        services.ConfigureMarten(
            options => { options.Projections.Add<SomeReadModel11Projection>(ProjectionLifecycle.Async); });

        martenConfiguration.AddSubscriptionWithServices<SomeSubscription11>(ServiceLifetime.Scoped);
        martenConfiguration.AddSubscriptionWithServices<SomeSubscription12>(ServiceLifetime.Scoped);

        return services;
    }

    public static IEndpointRouteBuilder MapSomeServicePart1Endpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("s1");
        group.MapPost("e11", E11);
        group.MapPut("e12/{streamId:guid}", E12);
        group.MapDelete("e13/{streamId:guid}", E13);
        return group;
    }

    private static async Task<Ok<string>> E11([FromServices] IDocumentSession documentSession, [FromQuery] string name,
        CancellationToken cancellationToken)
    {
        var streamId = Guid.NewGuid();
        var e11 = new Event11(streamId, name);
        documentSession.Events.StartStream(streamId, e11);
        await documentSession.SaveChangesAsync(cancellationToken);

        return TypedResults.Ok(streamId.ToString());
    }

    private static async Task<Ok<string>> E12([FromServices] IDocumentSession documentSession,
        [FromRoute] Guid streamId, [FromQuery] string name,
        CancellationToken cancellationToken)
    {
        var e12 = new Event12(streamId, name);
        documentSession.Events.Append(streamId, e12);
        await documentSession.SaveChangesAsync(cancellationToken);

        return TypedResults.Ok(streamId.ToString());
    }

    private static async Task<Ok<string>> E13([FromServices] IDocumentSession documentSession,
        [FromRoute] Guid streamId,
        CancellationToken cancellationToken)
    {
        var e13 = new Event13(streamId);
        documentSession.Events.Append(streamId, e13);
        await documentSession.SaveChangesAsync(cancellationToken);

        return TypedResults.Ok(streamId.ToString());
    }
}