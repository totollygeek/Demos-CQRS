namespace Todorov.Demos.CQRS.Infrastructure.Interfaces
{
    public interface ICommandBus
    {
        void Subscribe<T>(IHandleCommand<T> subscriber)
            where T : ICommand;
        void Publish<T>(T command)
            where T : ICommand;
    }
}
