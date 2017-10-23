using System;
namespace Todorov.Demos.CQRS.Infrastructure.Interfaces
{
    public interface IEventBus
    {
        void Subscribe<T>(Action<T> callback)
            where T : IEvent;

        void Publish<T>(T @event)
            where T : IEvent;
    }
}
