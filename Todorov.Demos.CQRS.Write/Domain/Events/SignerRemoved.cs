using Todorov.Demos.CQRS.Infrastructure;

namespace Todorov.Demos.CQRS.Write.Domain.Events
{
    public class SignerRemoved : VersionedEvent
    {
        public SignerRemoved(string email)
        {
            Email = email;
        }

        public string Email { get; }

        public override string ToString()
        {
            return $"({SourceId})[SignerRemoved: Email={Email}]";
        }
    }
}
