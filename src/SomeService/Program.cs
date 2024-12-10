using JasperFx.CodeGeneration;
using Marten;
using Marten.Events.Daemon.Resiliency;
using MassTransit;
using Npgsql;
using Oakton;
using SomeServicePart1;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var martenConfiguration = builder.Configuration.GetSection("Marten");
var martenCliEnabled = martenConfiguration.GetValue<bool>("CliEnabled");
var martenDaemonCliEnabled = martenConfiguration.GetValue<bool>("CliEnabled");
if (martenCliEnabled)
{
    builder.Host.ApplyOaktonExtensions();
}

builder.Services.AddMassTransit(cfg =>
{
    var rabbitMqConfigSection = builder.Configuration.GetSection("MassTransit:RabbitMq");
    cfg.UsingRabbitMq((context, rabbitMqConfigurator) =>
    {
        rabbitMqConfigurator.Host(
            rabbitMqConfigSection.GetValue<string>("Host"),
            rabbitMqConfigSection.GetValue<string>("VirtualHost"), h =>
            {
                h.Username(rabbitMqConfigSection.GetValue<string>("User")!);
                h.Password(rabbitMqConfigSection.GetValue<string>("Password")!);
            });

        rabbitMqConfigurator.ConfigureEndpoints(context);
    });
});

var martenconfiguration = builder.Services.AddMarten(options =>
        {
            StoreOptions storeOptions = new();
            storeOptions.Connection(builder.Configuration.GetConnectionString("DefaultConnection")!);
            storeOptions.DisableNpgsqlLogging = false;
            storeOptions.Events.DatabaseSchemaName = "events";
            storeOptions.DatabaseSchemaName = "documents";

            return storeOptions;
        }
    ).ApplyAllDatabaseChangesOnStartup()
    .UseLightweightSessions()
    .OptimizeArtifactWorkflow(
        TypeLoadMode.Static,
        builder.Configuration.GetValue<string>("Marten:DevelopmentEnvironment", "Development")!
    )
    .AddAsyncDaemon(DaemonMode.HotCold);
// .AddAsyncDaemon(martenDaemonCliEnabled ? DaemonMode.HotCold : DaemonMode.Disabled);

builder.Services
    .AddSomeServicePart1Services(martenconfiguration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapSomeServicePart1Endpoints();

await CreateDatabaseIfNotExistAsync();

if (martenCliEnabled)
{
    await app.RunOaktonCommands(args);
}
else
{
    await app.RunAsync();
}

async Task CreateDatabaseIfNotExistAsync()
{
    var connectionStringBuilder =
        new NpgsqlConnectionStringBuilder(builder.Configuration.GetConnectionString("DefaultConnection")!);
    var databaseName = connectionStringBuilder.Database;
    connectionStringBuilder.Database = "postgres";
    await using var connection = new NpgsqlConnection(connectionStringBuilder.ConnectionString);
    await connection.OpenAsync();

    await using var queryCmd = new NpgsqlCommand(
        $"SELECT datname FROM pg_database WHERE datname = '{databaseName}'",
        connection
    );

    var queryResult = await queryCmd.ExecuteScalarAsync();

    if (queryResult is null)
    {
        await using var createCmd = new NpgsqlCommand($"CREATE DATABASE \"{databaseName}\"", connection);
        await createCmd.ExecuteNonQueryAsync();
    }
}

public sealed partial class Program;