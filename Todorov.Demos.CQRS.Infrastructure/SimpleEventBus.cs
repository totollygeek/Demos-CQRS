using System;
using System.Collections.Generic;
using Todorov.Demos.CQRS.Infrastructure.Interfaces;

namespace Todorov.Demos.CQRS.Infrastructure
{
    public class SimpleEventBus : IEventBus
    {
        private readonly Dictionary<Type, List<Delegate>> _subscribers = new Dictionary<Type, List<Delegate>>();

        public void Subscribe<T>(Action<T> callback)
            where T : IVersionedEvent
        {
            if (_subscribers.TryGetValue(typeof(T), out List<Delegate> listeners))
            {
                listeners.Add(callback);
            }
            else
            {
                _subscribers.Add(typeof(T), new List<Delegate> { callback });
            }
        }

        public void Publish<T>(T @event)
            where T : IVersionedEvent
        {
            if (_subscribers.TryGetValue(typeof(T), out List<Delegate> listeners))
            {
                foreach (Action<T> callback in listeners)
                    callback(@event);
            }
        }
    }
}
