using Marten;
using Marten.Events.Daemon;
using Marten.Events.Daemon.Internals;
using Marten.Subscriptions;
using MassTransit;
using SomeServicePart1.Events;
using SomeServicePart1.Integration;

namespace SomeServicePart1.Subscriptions;

public class SomeSubscription11 : SubscriptionBase
{
    private readonly IBus _bus;

    public SomeSubscription11(IBus bus)
    {
        _bus = bus;

        SubscriptionName = "SomeSubscription11";
        SubscriptionVersion = 1;

        IncludeType<Event11>();
        IncludeType<Event12>();

        Options.SubscribeFromSequence(0);
    }

    public override async Task<IChangeListener> ProcessEventsAsync(EventRange page, ISubscriptionController controller,
        IDocumentOperations operations,
        CancellationToken cancellationToken)
    {
        foreach (var @event in page.Events)
        {
            switch (@event.Data)
            {
                case Event11 e11:
                    await _bus.Publish(new RmCreatedEvent(@event.StreamId, e11.Name), cancellationToken);
                    break;
                case Event12 e12:
                    await _bus.Publish(new RmRenamedEvent(@event.StreamId, e12.Name), cancellationToken);
                    break;
                default:
                    throw new InvalidOperationException("Unknown event type");
            }
        }

        return NullChangeListener.Instance;
    }
}