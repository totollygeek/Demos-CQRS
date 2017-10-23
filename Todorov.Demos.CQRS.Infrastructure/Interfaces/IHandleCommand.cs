namespace Todorov.Demos.CQRS.Infrastructure.Interfaces
{
    public interface IHandler {}
    public interface IHandleCommand<T> : IHandler
        where T : ICommand
    {
        void Handle(T command);
    }
}
