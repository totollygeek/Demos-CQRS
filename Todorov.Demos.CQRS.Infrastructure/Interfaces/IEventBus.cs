using System;
namespace Todorov.Demos.CQRS.Infrastructure.Interfaces
{
    public interface IEventBus
    {
        void Subscribe<T>(Action<T> callback)
            where T : IVersionedEvent;

        void Publish<T>(T @event)
            where T : IVersionedEvent;
    }
}
