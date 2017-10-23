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
        public PetitionCommandHandler()
        {
        }

        public void Handle(CreatePetition command)
        {

        }

        public void Handle(SignPetition command)
        {
            
        }

        public void Handle(RevokeSign command)
        {
            
        }

        public void Handle(ClosePetition command)
        {
            
        }
    }
}
