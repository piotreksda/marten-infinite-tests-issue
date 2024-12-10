using FluentAssertions;
using Marten;
using Microsoft.Extensions.DependencyInjection;
using SomeServicePart1.Events;
using SomeServicePart1.ReadModels;

namespace Integration;

[Collection(IntegrationCollection.Name)]
public sealed class SomeServicePart1Tests(IntegrationWebApplicationFactory factory)
{
    public static IEnumerable<object[]> GetTestData()
    {
        for (int i = 0; i < 25; i++)
        {
            yield return new object[] { Guid.NewGuid().ToString() };
        }
    }

    [Theory]
    [MemberData(nameof(GetTestData))]
    public async Task E11_TEST(string randomName)
    {
        var streamId = Guid.NewGuid();
        var e11 = new Event11(streamId, randomName);
        await using var scope = factory.Services.CreateAsyncScope();

        var documentSession = scope.ServiceProvider.GetRequiredService<IDocumentSession>();

        documentSession.Events.StartStream(streamId, e11);
        await documentSession.SaveChangesAsync();

        await factory.WaitForProjectionsCatchUpAsync();

        var readModel = await documentSession.LoadAsync<SomeReadModel11>(streamId);

        readModel.Should().NotBeNull();
        readModel!.Id.Should().Be(streamId);
        readModel!.Name.Should().Be(e11.Name);
    }
    //
    // [Theory]
    // [MemberData(nameof(GetTestData))]
    // public async Task E12_TEST(string randomName)
    // {
    //     var streamId = Guid.NewGuid();
    //     var e11 = new Event11(streamId, randomName);
    //     var e12 = new Event12(streamId, randomName + "2");
    //     await using var scope = factory.Services.CreateAsyncScope();
    //
    //     var documentSession = scope.ServiceProvider.GetRequiredService<IDocumentSession>();
    //
    //     documentSession.Events.StartStream(streamId, e11);
    //     documentSession.Events.Append(streamId, e12);
    //     await documentSession.SaveChangesAsync();
    //
    //     await factory.WaitForProjectionsCatchUpAsync();
    //
    //     var readModel = await documentSession.LoadAsync<SomeReadModel11>(streamId);
    //
    //     readModel.Should().NotBeNull();
    //     readModel!.Id.Should().Be(streamId);
    //     readModel!.Name.Should().Be(randomName + "2");
    // }
    //
    // [Theory]
    // [MemberData(nameof(GetTestData))]
    // public async Task E13_TEST(string randomName)
    // {
    //     var streamId = Guid.NewGuid();
    //     var e11 = new Event11(streamId, randomName);
    //     var e13 = new Event13(streamId);
    //     await using var scope = factory.Services.CreateAsyncScope();
    //
    //     var documentSession = scope.ServiceProvider.GetRequiredService<IDocumentSession>();
    //
    //     documentSession.Events.StartStream(streamId, e11);
    //     documentSession.Events.Append(streamId, e13);
    //     await documentSession.SaveChangesAsync();
    //
    //     await factory.WaitForProjectionsCatchUpAsync();
    //
    //     var readModel = await documentSession.LoadAsync<SomeReadModel11>(streamId);
    //
    //     readModel.Should().BeNull();
    // }
}