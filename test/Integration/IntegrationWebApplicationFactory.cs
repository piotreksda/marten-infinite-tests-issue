using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Integration;

public sealed class IntegrationWebApplicationFactory : WebApplicationFactory<Program>,
    IAsyncLifetime
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(
            configurationBuilder =>
            {
                var configuration = configurationBuilder.Build();
                var connectionString = configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
                var updatedConnectionString =
                    new NpgsqlConnectionStringBuilder(connectionString) { Database = "marten-issue-integration" }
                        .ConnectionString;

                configurationBuilder.AddInMemoryCollection(
                    [new KeyValuePair<string, string?>("ConnectionStrings:DefaultConnection", updatedConnectionString)]
                );
            }
        );

        builder.ConfigureTestServices(
            services =>
            {
                services.AddMassTransitTestHarness(
                    configurator => configurator.SetTestTimeouts(testInactivityTimeout: TimeSpan.FromSeconds(5))
                );
            }
        );

        base.ConfigureWebHost(builder);
    }

    public WebApplicationFactory<Program> WithWebHostBuilderTest(Action<IWebHostBuilder> configuration)
    {
        return WithWebHostBuilder(cfg =>
        {
            configuration.Invoke(cfg);
            cfg.UseSetting("Marten:DaemonEnabled", "false");
        });
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public new async Task DisposeAsync()
    {
        await base.DisposeAsync();
    }
}