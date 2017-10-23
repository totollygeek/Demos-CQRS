using System;
using System.Collections.Generic;
using System.Linq;
using Todorov.Demos.CQRS.Infrastructure.Interfaces;

namespace Todorov.Demos.CQRS.Infrastructure
{
    public class SimpleCommandBus : ICommandBus
    {
        private readonly Dictionary<Type, List<IHandler>> _subscribers = new Dictionary<Type, List<IHandler>>();

        public void Subscribe<T>(IHandleCommand<T> subscriber)
            where T : ICommand
        {
            if (_subscribers.TryGetValue(typeof(T), out List<IHandler> handlers))
            {
                handlers.Add(subscriber);
            }
            else
            {
                _subscribers.Add(typeof(T), new List<IHandler> { subscriber });
            }
        }

        public void Publish<T>(T command)
            where T : ICommand
        {
            if (_subscribers.TryGetValue(typeof(T), out List<IHandler> handlers))
            {   // We just get the first one from the subsctibed elements
                ((IHandleCommand<T>)handlers.FirstOrDefault())?.Handle(command);
            }
        }
    }
}
