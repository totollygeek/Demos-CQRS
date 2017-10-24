using System;
namespace Todorov.Demos.CQRS.Infrastructure.Interfaces
{
    public interface ICommand
    {
        Guid AggregateId { get; }
    }
}
