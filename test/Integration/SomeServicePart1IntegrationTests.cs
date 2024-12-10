using FluentAssertions;
using Marten;
using MassTransit.Testing;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using SomeServicePart1.Events;
using SomeServicePart1.Integration;

namespace Integration;

[Collection(IntegrationCollection.Name)]
public sealed class SomeServicePart1IntegrationTests : IAsyncLifetime
{
    private readonly WebApplicationFactory<Program> _factory;

    public SomeServicePart1IntegrationTests(IntegrationWebApplicationFactory factory)
    {
        _factory = factory.WithWebHostBuilder(x => x.UseSetting("abc:def:ghi", "true"));
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        await _factory.DisposeAsync();
    }

    public static IEnumerable<object[]> GetTestData()
    {
        for (int i = 0; i < 25; i++)
        {
            yield return [Guid.NewGuid().ToString()];
        }
    }

    [Theory]
    [MemberData(nameof(GetTestData))]
    public async Task E11_TEST(string randomName)
    {
        // var testHarness = _factory.Services.GetTestHarness();

        var streamId = Guid.NewGuid();
        var e11 = new Event11(streamId, randomName);
        await using var scope = _factory.Services.CreateAsyncScope();

        var documentSession = scope.ServiceProvider.GetRequiredService<IDocumentSession>();

        documentSession.Events.StartStream(streamId, e11);
        await documentSession.SaveChangesAsync();

        await _factory.WaitForProjectionsCatchUpAsync();

        // var published = await testHarness.Published.Any<RmCreatedEvent>(x => x.Context.Message.Id == streamId);
        // published.Should().BeTrue();
    }
}