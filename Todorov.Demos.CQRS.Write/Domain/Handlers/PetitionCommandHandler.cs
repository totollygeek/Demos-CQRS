using System;
using Todorov.Demos.CQRS.Infrastructure.Interfaces;
using Todorov.Demos.CQRS.Write.Domain.Commands;

namespace Todorov.Demos.CQRS.Write.Domain.Handlers
{
    public class PetitionCommandHandler :
        IHandleCommand<CreatePetition>,
        IHandleCommand<SignPetition>,
        IHandleCommand<RevokeSign>,
        IHandleCommand<ClosePetition>
    {
        private readonly IEventSourceRepository<PetitionAggregate> _repository;
        public PetitionCommandHandler(IEventSourceRepository<PetitionAggregate> repository)
        {
            _repository = repository;
        }

        public void Handle(CreatePetition command)
        {
            var aggregate = _repository.Find(command.AggregateId);
            if (aggregate != null)
                throw new InvalidOperationException($"Petition with id {command.AggregateId} is already created");

            aggregate = new PetitionAggregate(command.AggregateId, command.Title, command.StartDate);
            _repository.Save(aggregate);
        }

        public void Handle(SignPetition command)
        {
            var aggregate = GetAggregate(command.AggregateId);
            aggregate.AddSigner(command.Email, command.FirstName, command.LastName);
            _repository.Save(aggregate);
        }

        public void Handle(RevokeSign command)
        {
            var aggregate = GetAggregate(command.AggregateId);
            aggregate.RemoveSigner(command.Email);
            _repository.Save(aggregate);
        }

        public void Handle(ClosePetition command)
        {
            var aggregate = GetAggregate(command.AggregateId);
            aggregate.ClosePetition(command.StopDate);
            _repository.Save(aggregate);
        }

        private PetitionAggregate GetAggregate(Guid id)
        {
            var aggregate = _repository.Find(id);

            if (aggregate == null)
                throw new InvalidOperationException($"Petition with id {id} was not found!");

            return aggregate;
        }
    }
}
