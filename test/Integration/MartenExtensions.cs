using Marten;
using Marten.Events;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Integration;

public static class MartenExtensions
{
    public static async Task WaitForProjectionsCatchUpAsync(
        this WebApplicationFactory<Program> webApplicationFactory,
        int timeoutInSeconds = 10
    )
    {
        var documentStore = webApplicationFactory.Services.GetRequiredService<IDocumentStore>();
        await documentStore.WaitForNonStaleProjectionDataAsync(TimeSpan.FromSeconds(timeoutInSeconds));
    }
}