using System;
using Todorov.Demos.CQRS.Infrastructure;

namespace Todorov.Demos.CQRS.Write.Domain.Events
{
    public class PetitionFinished : VersionedEvent
    {
        public PetitionFinished(DateTime finishDate)
        {
            FinishDate = finishDate;
        }

        public DateTime FinishDate { get; }

        public override string ToString()
        {
            return $"({SourceId})[PetitionFinished: FinishDate={FinishDate}]";
        }
    }
}
