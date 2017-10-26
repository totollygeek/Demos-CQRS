using System;
using System.Collections.Generic;
using System.Linq;
using Todorov.Demos.CQRS.Infrastructure.Interfaces;
using Todorov.Demos.CQRS.Read.Models;

namespace Todorov.Demos.CQRS.Read
{
    public class InMemoryReadModelStore : IReadModelStore<PetitionModel>
    {
        private static readonly Dictionary<Guid, PetitionModel> _store = new Dictionary<Guid, PetitionModel>();

        public PetitionModel Get(Guid id)
        {
            if (_store.TryGetValue(id, out PetitionModel model))
                return PetitionModel.Clone(model);

            return null;
        }

        public PetitionModel[] GetAll()
        {
            return _store.Values.ToArray();
        }

        public void Save(PetitionModel model)
        {
            _store[model.Id] = model;
        }
    }
}
