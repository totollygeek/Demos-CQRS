using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Todorov.Demos.CQRS.Infrastructure.Interfaces;

namespace Todorov.Demos.CQRS.Infrastructure
{
    public class InMemoryEventSourceRepository<TAggregate> : IEventSourceRepository<TAggregate>
        where TAggregate : class, IAggregate
    {
        private static readonly Dictionary<Guid, List<IVersionedEvent>> _events = new Dictionary<Guid, List<IVersionedEvent>>();

        private readonly IEventBus _eventBus;
        private readonly Action<string[]> _addEventsCallback;
        private static readonly Func<Guid, IReadOnlyList<IVersionedEvent>, TAggregate> AggregateFactory;

        static InMemoryEventSourceRepository()
        {
            var constructor = typeof(TAggregate).GetConstructor(new[] { typeof(Guid), typeof(IReadOnlyList<IVersionedEvent>) });
            var idParameter = Expression.Parameter(typeof(Guid));
            var eventsParameter = Expression.Parameter(typeof(IReadOnlyList<IVersionedEvent>));

            var expression =
                Expression.Lambda<Func<Guid, IReadOnlyList<IVersionedEvent>, TAggregate>>(
                Expression.New(constructor, idParameter, eventsParameter),
                    idParameter,
                    eventsParameter);
            AggregateFactory = expression.Compile();
        }

        public InMemoryEventSourceRepository(IEventBus eventBus, Action<string[]> addEventsCallback)
        {
            _addEventsCallback = addEventsCallback;
        }

        public TAggregate Find(Guid id)
        {
            if (_events.TryGetValue(id, out List<IVersionedEvent> foundEvents))
            {
                var ordered = foundEvents.OrderBy(e => e.Version).ToArray();

                return AggregateFactory(id, ordered);
            }

            return null;
        }

        public void Save(TAggregate aggregate)
        {
            List<IVersionedEvent> dbEvents;
            if (_events.ContainsKey(aggregate.Id))
                dbEvents = _events[aggregate.Id];
            else
            {
                dbEvents = new List<IVersionedEvent>();
                _events.Add(aggregate.Id, dbEvents);
            }

            var output = new List<string>();
            foreach (var @event in aggregate.PendingEvents)
            {
                dbEvents.Add(@event);
                output.Add(@event.ToString());
                _eventBus.Publish(@event);
            }

            _addEventsCallback(output.ToArray());
        }
    }
}
