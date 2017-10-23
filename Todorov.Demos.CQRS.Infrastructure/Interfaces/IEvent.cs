using System;
namespace Todorov.Demos.CQRS.Infrastructure.Interfaces
{
    public interface IEvent
    {
        int UpdateFromSource(Guid sourceId, int sourceVersion);
        Guid SourceId { get; }
        int Version { get; }
    }
}
