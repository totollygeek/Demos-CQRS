using Todorov.Demos.CQRS.Infrastructure;

namespace Todorov.Demos.CQRS.Write.Domain.Events
{
    public class SignersCountChanged : VersionedEvent
    {
        public SignersCountChanged(int newCount)
        {
            NewCount = newCount;
        }

        public int NewCount { get; }
    }
}
