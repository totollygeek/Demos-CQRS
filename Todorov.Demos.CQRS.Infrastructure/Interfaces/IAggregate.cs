using System;
using System.Collections.Generic;

namespace Todorov.Demos.CQRS.Infrastructure.Interfaces
{
    public interface IAggregate
    {
        Guid Id { get; }
        IReadOnlyList<IVersionedEvent> PendingEvents { get; }
    }
}
