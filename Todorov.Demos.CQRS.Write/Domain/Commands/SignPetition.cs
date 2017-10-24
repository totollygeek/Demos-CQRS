using System;
using Todorov.Demos.CQRS.Infrastructure.Interfaces;

namespace Todorov.Demos.CQRS.Write.Domain.Commands
{
    public class SignPetition : ICommand
    {
        public SignPetition(Guid aggregateId, string email, string firstName, string lastName)
        {
            AggregateId = aggregateId;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
        }

        public Guid AggregateId { get; }
        public string Email { get; }   
        public string FirstName { get; }
        public string LastName { get; }    

        public override string ToString()
        {
            return $"[SignPetition: Email={Email}, FirstName={FirstName}, LastName={LastName}]";
        }
    }
}
