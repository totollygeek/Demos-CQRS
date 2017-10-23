using System;
namespace Todorov.Demos.CQRS.Infrastructure.Interfaces
{
    public interface IVersionedEvent
    {
        int UpdateFromSource(Guid sourceId, int sourceVersion);
        Guid SourceId { get; }
        int Version { get; }
        string Payload { get; }
    }
}
