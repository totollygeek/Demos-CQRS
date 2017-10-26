using System;
using Todorov.Demos.CQRS.Infrastructure;

namespace Todorov.Demos.CQRS.Write.Domain.Events
{
    public class PetitionCreated : VersionedEvent
    {
        public PetitionCreated(string title, DateTime startDate)
        {
            Title = title;
            StartDate = startDate;
        }

        public string Title { get; }
        public DateTime StartDate { get; }

        public override string ToString()
        {
            return $"[PetitionCreated: Title={Title}, StartDate={StartDate}]";
        }
    }
}
