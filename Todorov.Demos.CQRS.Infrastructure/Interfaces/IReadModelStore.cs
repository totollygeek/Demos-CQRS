using System;
namespace Todorov.Demos.CQRS.Infrastructure.Interfaces
{
    public interface IReadModelStore<TReadModel>
        where TReadModel : IReadModel
    {
        TReadModel Get(Guid id);
        TReadModel[] GetAll();
        void Save(TReadModel model);
    }
}
