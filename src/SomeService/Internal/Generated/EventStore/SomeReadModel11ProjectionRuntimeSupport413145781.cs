// <auto-generated/>
#pragma warning disable
using Marten;
using Marten.Events.Aggregation;
using Marten.Internal.Storage;
using SomeServicePart1.Projections;
using System;
using System.Linq;

namespace Marten.Generated.EventStore
{
    // START: SomeReadModel11ProjectionLiveAggregation413145781
    public class SomeReadModel11ProjectionLiveAggregation413145781 : Marten.Events.Aggregation.SyncLiveAggregatorBase<SomeServicePart1.ReadModels.SomeReadModel11>
    {
        private readonly SomeServicePart1.Projections.SomeReadModel11Projection _someReadModel11Projection;

        public SomeReadModel11ProjectionLiveAggregation413145781(SomeServicePart1.Projections.SomeReadModel11Projection someReadModel11Projection)
        {
            _someReadModel11Projection = someReadModel11Projection;
        }



        public override SomeServicePart1.ReadModels.SomeReadModel11 Build(System.Collections.Generic.IReadOnlyList<Marten.Events.IEvent> events, Marten.IQuerySession session, SomeServicePart1.ReadModels.SomeReadModel11 snapshot)
        {
            if (!events.Any()) return snapshot;
            var usedEventOnCreate = snapshot is null;
            snapshot ??= Create(events[0], session);;
            if (snapshot is null)
            {
                usedEventOnCreate = false;
                snapshot = CreateDefault(events[0]);
            }

            foreach (var @event in events.Skip(usedEventOnCreate ? 1 : 0))
            {
                snapshot = Apply(@event, snapshot, session);
            }

            return snapshot;
        }


        public SomeServicePart1.ReadModels.SomeReadModel11 Create(Marten.Events.IEvent @event, Marten.IQuerySession session)
        {
            switch (@event)
            {
                case Marten.Events.IEvent<SomeServicePart1.Events.Event11> event_Event111:
                    return SomeServicePart1.Projections.SomeReadModel11Projection.Create(event_Event111);
                    break;
            }

            return null;
        }


        public SomeServicePart1.ReadModels.SomeReadModel11 Apply(Marten.Events.IEvent @event, SomeServicePart1.ReadModels.SomeReadModel11 aggregate, Marten.IQuerySession session)
        {
            switch (@event)
            {
                case Marten.Events.IEvent<SomeServicePart1.Events.Event12> event_Event122:
                    aggregate = _someReadModel11Projection.Apply(event_Event122, aggregate);
                    break;
            }

            return aggregate;
        }

    }

    // END: SomeReadModel11ProjectionLiveAggregation413145781
    
    
    // START: SomeReadModel11ProjectionInlineHandler413145781
    public class SomeReadModel11ProjectionInlineHandler413145781 : Marten.Events.Aggregation.AggregationRuntime<SomeServicePart1.ReadModels.SomeReadModel11, System.Guid>
    {
        private readonly Marten.IDocumentStore _store;
        private readonly Marten.Events.Aggregation.IAggregateProjection _projection;
        private readonly Marten.Events.Aggregation.IEventSlicer<SomeServicePart1.ReadModels.SomeReadModel11, System.Guid> _slicer;
        private readonly Marten.Internal.Storage.IDocumentStorage<SomeServicePart1.ReadModels.SomeReadModel11, System.Guid> _storage;
        private readonly SomeServicePart1.Projections.SomeReadModel11Projection _someReadModel11Projection;

        public SomeReadModel11ProjectionInlineHandler413145781(Marten.IDocumentStore store, Marten.Events.Aggregation.IAggregateProjection projection, Marten.Events.Aggregation.IEventSlicer<SomeServicePart1.ReadModels.SomeReadModel11, System.Guid> slicer, Marten.Internal.Storage.IDocumentStorage<SomeServicePart1.ReadModels.SomeReadModel11, System.Guid> storage, SomeServicePart1.Projections.SomeReadModel11Projection someReadModel11Projection) : base(store, projection, slicer, storage)
        {
            _store = store;
            _projection = projection;
            _slicer = slicer;
            _storage = storage;
            _someReadModel11Projection = someReadModel11Projection;
        }



        public override async System.Threading.Tasks.ValueTask<SomeServicePart1.ReadModels.SomeReadModel11> ApplyEvent(Marten.IQuerySession session, Marten.Events.Projections.EventSlice<SomeServicePart1.ReadModels.SomeReadModel11, System.Guid> slice, Marten.Events.IEvent evt, SomeServicePart1.ReadModels.SomeReadModel11 aggregate, System.Threading.CancellationToken cancellationToken)
        {
            switch (evt)
            {
                case Marten.Events.IEvent<SomeServicePart1.Events.Event11> event_Event116:
                    aggregate = SomeServicePart1.Projections.SomeReadModel11Projection.Create(event_Event116);
                    return aggregate;
                case Marten.Events.IEvent<SomeServicePart1.Events.Event12> event_Event125:
                    aggregate ??= CreateDefault(evt);
                    aggregate = _someReadModel11Projection.Apply(event_Event125, aggregate);
                    return aggregate;
                case Marten.Events.IEvent<SomeServicePart1.Events.Event13> event_Event134:
                    return null;
                    aggregate ??= CreateDefault(evt);
                    return aggregate;
            }

            return aggregate;
        }


        public SomeServicePart1.ReadModels.SomeReadModel11 Create(Marten.Events.IEvent @event, Marten.IQuerySession session)
        {
            switch (@event)
            {
                case Marten.Events.IEvent<SomeServicePart1.Events.Event11> event_Event113:
                    return SomeServicePart1.Projections.SomeReadModel11Projection.Create(event_Event113);
                    break;
            }

            return null;
        }

    }

    // END: SomeReadModel11ProjectionInlineHandler413145781
    
    
}

