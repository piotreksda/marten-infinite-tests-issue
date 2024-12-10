namespace Integration;

[CollectionDefinition(Name)]
public class IntegrationCollection : ICollectionFixture<IntegrationWebApplicationFactory>
{
    public const string Name = "integration";
}