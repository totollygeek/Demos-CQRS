using Todorov.Demos.CQRS.Infrastructure.Interfaces;
using Todorov.Demos.CQRS.Read.Models;

namespace Todorov.Demos.CQRS.Read
{
    public class PetitionQueryService
    {
        private readonly IReadModelStore<PetitionModel> _modelStore;
        public PetitionQueryService(IReadModelStore<PetitionModel> modelStore)
        {
            _modelStore = modelStore;
        }

        public PetitionModel[] GetAll()
        {
            return _modelStore.GetAll();
        }		
    }
}
