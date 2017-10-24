using System;
using Todorov.Demos.CQRS.Infrastructure.Interfaces;

namespace Todorov.Demos.CQRS.Write.Domain.Commands
{
    public class ClosePetition : ICommand
    {
        public ClosePetition(Guid aggregateId)
        {
            AggregateId = aggregateId;
        }

        public Guid AggregateId { get; }

        public override string ToString()
        {
            return $"[ClosePetition: Id={AggregateId}]";
        }
    }
}
