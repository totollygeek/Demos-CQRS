using System;
using System.Collections.Generic;
using Todorov.Demos.CQRS.Infrastructure.Interfaces;
using Todorov.Demos.CQRS.Write.Domain.Events;

namespace Todorov.Demos.CQRS.Write.Domain
{
    public class PetitionAggregate : IAggregate
    {
        private readonly List<IVersionedEvent> _pendingEvents = new List<IVersionedEvent>();
        private Dictionary<Type, Action<IVersionedEvent>> _handlers = new Dictionary<Type, Action<IVersionedEvent>>();

        #region Constructors
        public PetitionAggregate(Guid id, string title, DateTime startDate)
            : this(id)
        {
            Update(new PetitionCreated(id, title, startDate));
        }

        public PetitionAggregate(Guid id, IReadOnlyList<IVersionedEvent> events)
            : this(id)
        {
            LoadFrom(events);
        }

        private PetitionAggregate(Guid id)
        {
            Id = id;
            State = AggregateState.NotStarted;

            Handle<PetitionCreated>(OnPetitionCreated);
            Handle<SignerAdded>(OnSignerAdded);
            Handle<SignersCountChanged>(OnSignersCountChanged);
            Handle<SignerRemoved>(OnSignerRemoved);
            Handle<PetitionFinished>(OnPetitionFinished);
        }
        #endregion

        #region Events Handling
        private void LoadFrom(IReadOnlyList<IVersionedEvent> events)
        {
            for (var i = 0; i < events.Count; i++)
            {
                var e = events[i];
                if (_handlers.TryGetValue(e.GetType(), out Action<IVersionedEvent> callback))
                {
                    callback(e);
                }
                else
                    throw new NotSupportedException($"{e.GetType()} is not a supported event type for PetitionAggregate");
            }
        }

        private void Handle<TEvent>(Action<TEvent> callback)
            where TEvent : IVersionedEvent
        {
            _handlers.Add(typeof(TEvent), @event => callback((TEvent)@event));
        }

        private void Update(IVersionedEvent @event)
        {
            Version = @event.UpdateFromSource(Id, Version);
            _pendingEvents.Add(@event);
        }
        #endregion

        #region State
        public Guid Id { get; }
        public int Version { get; private set; }
        public IReadOnlyList<IVersionedEvent> PendingEvents => _pendingEvents;

        public AggregateState State { get; private set; }
        public string Title { get; private set; }
        public DateTime? StartDate { get; private set; }
        public int SignedCount { get; private set; }
        public List<string> Signers { get; private set; }
        public DateTime? FinishDate { get; private set; }
        #endregion       

        #region Events Callbacks
        private void OnPetitionCreated(PetitionCreated @event)
        {
            Title = @event.Title;
            StartDate = @event.StartDate;
            State = AggregateState.Running;
        }

        private void OnSignerAdded(SignerAdded @event)
        {
            Signers.Add(@event.Email);
        }

        private void OnSignersCountChanged(SignersCountChanged @event)
        {
            SignedCount = @event.NewCount;
        }

        private void OnSignerRemoved(SignerRemoved @event)
        {
            if (!Signers.Contains(@event.Email))
                throw new InvalidOperationException($"Signer with email {@event.Email} was not found among signers");

            Signers.Remove(@event.Email);
        }

        private void OnPetitionFinished(PetitionFinished @event)
        {
            State = AggregateState.Finished;
            FinishDate = @event.FinishDate;
        }
        #endregion

        #region Public Methods
        public void AddSigner(string email, string firstName, string lastName)
        {
            Update(new SignerAdded(email, firstName, lastName));
            Update(new SignersCountChanged(Signers.Count + 1));
        }

        public void RemoveSigner(string email)
        {
            Update(new SignerRemoved(email));
            Update(new SignersCountChanged(Signers.Count - 1));
        }

        public void ClosePetition()
        {
            Update(new PetitionFinished(DateTime.UtcNow));
        }
        #endregion
    }

    public enum AggregateState
    {
        NotStarted,
        Running,
        Finished
    }
}
