using System;
using Todorov.Demos.CQRS.Infrastructure;

namespace Todorov.Demos.CQRS.Write.Domain.Events
{
    public class PetitionCreated : VersionedEvent
    {
        public PetitionCreated(Guid id, string title, DateTime startDate)
        {
            Id = id;
            Title = title;
            StartDate = startDate;
        }

        public Guid Id { get; }
        public string Title { get; }
        public DateTime StartDate { get; }

        public override string ToString()
        {
            return string.Format("[PetitionCreated: Id={0}, Title={1}, StartDate={2}]", Id, Title, StartDate);
        }
    }
}
