using Marten;
using Marten.Events.Daemon;
using Marten.Events.Daemon.Internals;
using Marten.Subscriptions;
using MassTransit;
using SomeServicePart1.Events;
using SomeServicePart1.Integration;

namespace SomeServicePart1.Subscriptions;

public class SomeSubscription12 : SubscriptionBase
{
    private readonly IBus _bus;

    public SomeSubscription12(IBus bus)
    {
        _bus = bus;

        SubscriptionName = "SomeSubscription12";
        SubscriptionVersion = 1;

        IncludeType<Event13>();

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
                case Event13 e13:
                    await _bus.Publish(new RmDeletedEvent(@event.StreamId), cancellationToken);
                    break;
                default:
                    throw new InvalidOperationException("Unknown event type");
            }
        }

        return NullChangeListener.Instance;
    }
}