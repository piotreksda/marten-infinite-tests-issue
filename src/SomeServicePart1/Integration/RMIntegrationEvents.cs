using Shared;

namespace SomeServicePart1.Integration;

public sealed record RmCreatedEvent(Guid Id, string Name) : IIntegrationEvent;

public sealed record RmRenamedEvent(Guid Id, string Name) : IIntegrationEvent;

public sealed record RmDeletedEvent(Guid Id) : IIntegrationEvent;