using System;
using Todorov.Demos.CQRS.Infrastructure.Interfaces;
using Todorov.Demos.CQRS.Write.Domain.Commands;

namespace Todorov.Demos.CQRS.Write
{
    public class PetitionService
    {
        private readonly ICommandBus _commandBus;
        public PetitionService(ICommandBus commandBus)
        {
            _commandBus = commandBus;
        }

        public void CreatePetition(string title)
        {
            _commandBus.Publish(new CreatePetition(Guid.NewGuid(), title, DateTime.UtcNow));
        }

        public void SignPetition(Guid petitionId, string email, string firstName, string lastName)
        {
            _commandBus.Publish(new SignPetition(petitionId, email, firstName, lastName));
        }

        public void RevokeSign(Guid petitionId, string email)
        {
            _commandBus.Publish(new RevokeSign(petitionId, email));
        }

        public void ClosePetition(Guid petitionId)
        {
            _commandBus.Publish(new ClosePetition(petitionId, DateTime.UtcNow));
        }
    }
}
