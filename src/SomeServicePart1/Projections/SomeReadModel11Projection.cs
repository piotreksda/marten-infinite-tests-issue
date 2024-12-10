using Marten.Events;
using Marten.Events.Aggregation;
using SomeServicePart1.Events;
using SomeServicePart1.ReadModels;

namespace SomeServicePart1.Projections;

public class SomeReadModel11Projection : SingleStreamProjection<SomeReadModel11>
{
    public SomeReadModel11Projection()
    {
        ProjectionName = "SomeReadModel11Projection";

        DeleteEvent<Event13>();
    }

    public static SomeReadModel11 Create(IEvent<Event11> e) =>
        new(
            Guid.NewGuid(),
            e.Data.Name
        );

    public SomeReadModel11 Apply(IEvent<Event12> e, SomeReadModel11 p) =>
        p with { Name = e.Data.Name };
}