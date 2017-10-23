using System;
using Todorov.Demos.CQRS.Infrastructure.Interfaces;

namespace Todorov.Demos.CQRS.Write.Domain.Commands
{
    public class ClosePetition : ICommand
    {
        public ClosePetition(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
