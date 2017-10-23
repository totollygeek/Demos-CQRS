using System;
using Todorov.Demos.CQRS.Infrastructure.Interfaces;

namespace Todorov.Demos.CQRS.Infrastructure
{
    public abstract class VersionedEvent : IVersionedEvent
    {
        protected VersionedEvent()
        {
        }

        public int UpdateFromSource(Guid sourceId, int sourceVersion)
        {
            SourceId = sourceId;
            Version = sourceVersion + 1;
            return Version;
        }

        public Guid SourceId { get; private set; }
        public int Version { get; private set; }
        public string Payload => ToString();
    }
}
