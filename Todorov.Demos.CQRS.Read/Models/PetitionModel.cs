using System;
using System.Collections.Generic;
using Todorov.Demos.CQRS.Infrastructure.Interfaces;

namespace Todorov.Demos.CQRS.Read.Models
{
    public class PetitionModel : IReadModel
    {
        public static PetitionModel Clone(PetitionModel model)
        {
            return new PetitionModel()
            {
                Id = model.Id,
                Title = model.Title,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Signers = new Dictionary<string, PetitionSigner>(model.Signers)
            };
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int SignersCount { get; set; }
        public PetitionState State { get; set; }
        public Dictionary<string, PetitionSigner> Signers { get; set; } = new Dictionary<string, PetitionSigner>();

        public override string ToString()
        {
            return $"[PetitionModel: Id={Id}, Title={Title}, StartDate={StartDate}, EndDate={EndDate}]";
        }
    }

    public enum PetitionState
    {
        Running,
        Finished
    }
}
