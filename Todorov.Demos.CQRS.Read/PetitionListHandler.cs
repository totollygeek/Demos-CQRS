using Todorov.Demos.CQRS.Infrastructure.Interfaces;
using Todorov.Demos.CQRS.Read.Models;
using Todorov.Demos.CQRS.Write.Domain.Events;

namespace Todorov.Demos.CQRS.Read
{
    public class PetitionListHandler
    {
        private readonly IReadModelStore<PetitionModel> _modelStore;
        public PetitionListHandler(IEventBus eventBus, IReadModelStore<PetitionModel>  modelStore)
        {
            _modelStore = modelStore;

            eventBus.Subscribe<PetitionCreated>(OnPetitionCreated);
            eventBus.Subscribe<PetitionFinished>(OnPetitionFinished);
            eventBus.Subscribe<SignerAdded>(OnSignerAdded);
            eventBus.Subscribe<SignerRemoved>(OnSignerRemoved);
            eventBus.Subscribe<SignersCountChanged>(OnSignerCountChanged);
        }

        private void OnPetitionCreated(PetitionCreated @event)
        {
            var model = new PetitionModel
            {
                Id = @event.SourceId,
                Title = @event.Title,
                StartDate = @event.StartDate
            };

            _modelStore.Save(model);
        }

        private void OnPetitionFinished(PetitionFinished @event)
        {
            var model = _modelStore.Get(@event.SourceId);
            model.EndDate = @event.FinishDate;
            _modelStore.Save(model);
        }

        private void OnSignerAdded(SignerAdded @event)
        {
            var model = _modelStore.Get(@event.SourceId);
            model.Signers.Add(@event.Email, new PetitionSigner(@event.Email, @event.FirstName, @event.LastName));
            _modelStore.Save(model);
        }

        private void OnSignerRemoved(SignerRemoved @event)
        {
            var model = _modelStore.Get(@event.SourceId);
            model.Signers.Remove(@event.Email);
            _modelStore.Save(model);
        }

        private void OnSignerCountChanged(SignersCountChanged @event)
        {
            var model = _modelStore.Get(@event.SourceId);
            model.SignersCount = @event.NewCount;
            _modelStore.Save(model);
        }
    }
}
