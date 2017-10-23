using System;
using Todorov.Demos.CQRS.Infrastructure.Interfaces;

namespace Todorov.Demos.CQRS.Write.Domain.Commands
{
    public class CreatePetition : ICommand
    {
        public CreatePetition(Guid id, string title, DateTime startDate)
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
            return string.Format("[CreatePetition: Id={0}, Title={1}, StartDate={2}]", Id, Title, StartDate);
        }
    }
}
