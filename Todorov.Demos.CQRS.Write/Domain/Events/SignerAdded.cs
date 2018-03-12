using System;
using Todorov.Demos.CQRS.Infrastructure;

namespace Todorov.Demos.CQRS.Write.Domain.Events
{
    public class SignerAdded : VersionedEvent
    {
        public SignerAdded(string email, string firstName, string lastName)
        {
            Email = email;
            FirstName = firstName;
            LastName = lastName;
        }

        public string Email { get; }   
        public string FirstName { get; }
        public string LastName { get; }    

        public override string ToString()
        {
            return $"({SourceId})[SignerAdded: Email={Email}, FirstName={FirstName}, LastName={LastName}]";
        }
    }
}
