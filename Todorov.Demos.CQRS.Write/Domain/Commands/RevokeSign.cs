using System;
using Todorov.Demos.CQRS.Infrastructure.Interfaces;

namespace Todorov.Demos.CQRS.Write.Domain.Commands
{
    public class RevokeSign : ICommand
    {
        public RevokeSign(Guid aggregateId, string email)
        {
            AggregateId = aggregateId;
            Email = email;
        }

        public Guid AggregateId { get; }
        public string Email { get; }

        public override string ToString()
        {
            return $"[RevokeSign: Email={Email}]";
        }
    }
}
