using System;
using Todorov.Demos.CQRS.Infrastructure.Interfaces;

namespace Todorov.Demos.CQRS.Write.Domain.Commands
{
    public class CreatePetition : ICommand
    {
        public CreatePetition(Guid aggregateId, string title, DateTime startDate)
        {
            AggregateId = aggregateId;
            Title = title;
            StartDate = startDate;
        }

        public Guid AggregateId { get; }
        public string Title { get; }
        public DateTime StartDate { get; }

        public override string ToString()
        {
            return $"[CreatePetition: AggregateId={AggregateId}, Title={Title}, StartDate={StartDate}]";
        }
    }
}
