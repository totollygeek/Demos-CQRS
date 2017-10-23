using System;
namespace Todorov.Demos.CQRS.Infrastructure.Interfaces
{
    public interface IEventSourceRepository<TAggregate>
        where TAggregate : IAggregate
    {
        TAggregate Find(Guid id);
        void Save(TAggregate aggregate);
    }
}
