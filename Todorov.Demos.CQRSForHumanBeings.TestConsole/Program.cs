using System;
using System.Collections.Generic;
using Todorov.Demos.CQRS.Infrastructure;
using Todorov.Demos.CQRS.Infrastructure.Interfaces;
using Todorov.Demos.CQRS.Read;
using Todorov.Demos.CQRS.Read.Models;
using Todorov.Demos.CQRS.Write;
using Todorov.Demos.CQRS.Write.Domain;
using Todorov.Demos.CQRS.Write.Domain.Commands;
using Todorov.Demos.CQRS.Write.Domain.Handlers;

namespace Todorov.Demos.CQRS.TestConsole
{
    class Program
    {
        #region Private members
        private static List<IVersionedEvent> _eventsQueue = new List<IVersionedEvent>();

		private static ConsoleColor[] PetitionColors { get; } =
		{
			ConsoleColor.Cyan,
			ConsoleColor.Green,
			ConsoleColor.Magenta
		};

		private static int CurrentColor { get; set; } = 0;

		private static Dictionary<Guid, ConsoleColor> ColorMap { get; } = new Dictionary<Guid, ConsoleColor>();

		private static IEventBus _eventBus = new SimpleEventBus();
        private static ICommandBus _commandBus = new SimpleCommandBus();

        private static IEventSourceRepository<PetitionAggregate> _eventSourceRepository =
            new InMemoryEventSourceRepository<PetitionAggregate>(_eventBus, events => _eventsQueue.AddRange(events));
        private static IReadModelStore<PetitionModel> _readModelStore =
            new InMemoryReadModelStore();

        private static PetitionCommandHandler _commandHandler = new PetitionCommandHandler(_eventSourceRepository);
        private static PetitionListHandler _petitionListHandler = new PetitionListHandler(_eventBus, _readModelStore);

        private static PetitionService _service = new PetitionService(_commandBus);
        private static PetitionQueryService _queryService = new PetitionQueryService(_readModelStore);
        #endregion

        static void Main(string[] args)
        {
            Initialize();

            var choice = "";
            while(choice != "9")
            {
                ShowMenu();
                choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CreatePetition();
                        break;
                    case "2":
                        ViewPetitions();
                        break;
                    case "3":
                        SignPetition();
                        break;
                    case "4":
                        RevokeSign();
                        break;
                    case "5":
                        ClosePetition();
                        break;
                    case "6":
                        ViewEvents();
                        break;
                }
            }
            Console.WriteLine();
            Console.Write("Bye Bye!");
        }

        private static void CreatePetition()
        {
            Console.WriteLine();
            Console.Write("Enter title: ");
            var title = Console.ReadLine();

            _service.CreatePetition(title);

            Console.WriteLine("Petition created!");
            Console.WriteLine("Press any key to return to the menu...");
            Console.ReadKey();
        }

        private static void ViewPetitions()
        {
            Console.WriteLine();
            Console.WriteLine();

            var petitions = _queryService.GetAll();

            for (var i = 0; i < petitions.Length; i++)
            {
                var petition = petitions[i];
                Console.WriteLine($"{i + 1}) {petition.Title}");
                Console.WriteLine($"Started: {petition.StartDate}");
                var ended = petition.EndDate.HasValue ? petition.EndDate.Value.ToString() : "Not ended yet";
                Console.WriteLine($"Ended: {ended}");
                Console.WriteLine($"Status: {petition.State}");
                Console.WriteLine($"Signers: {petition.SignersCount}");
                Console.WriteLine("================ Signers ================");

				if (petition.Signers.Count == 0) Console.WriteLine("No signers yet!");
				else
				{
					var idx = 1;
					foreach (var signer in petition.Signers)
					{
						Console.WriteLine($"{idx}) {signer.Value.FirstName} {signer.Value.LastName} ({signer.Value.Email})");
						idx++;
					}
				}

                Console.WriteLine("=========================================");
            } 

            Console.WriteLine("Press any key to return to the menu...");
            Console.ReadKey();
        }

        private static void SignPetition()
        {
            Console.WriteLine();
            Guid petitionGuid = GetPetitionGuid();

            Console.WriteLine();
            Console.Write("Email: ");
            var email = Console.ReadLine();

            Console.Write("First name: ");
            var firstName = Console.ReadLine();

            Console.Write("Last name: ");
            var lastName = Console.ReadLine();

            _service.SignPetition(petitionGuid, email, firstName, lastName);

            Console.WriteLine("Petition signed!");
            Console.WriteLine("Press any key to return to the menu...");
            Console.ReadKey();
        }

        private static void RevokeSign()
        {
            Console.WriteLine();
            Guid petitionGuid = GetPetitionGuid();

            Console.WriteLine();
            Console.Write("Email: ");
            var email = Console.ReadLine();

            _service.RevokeSign(petitionGuid, email);

            Console.WriteLine("Petition sign revoked!");
            Console.WriteLine("Press any key to return to the menu...");
            Console.ReadKey();
        }

        private static void ClosePetition()
        {
            Console.WriteLine();
            Guid petitionGuid = GetPetitionGuid();

            _service.ClosePetition(petitionGuid);

            Console.WriteLine("Petition closed!");
            Console.WriteLine("Press any key to return to the menu...");
            Console.ReadKey();
        }

        private static void ViewEvents()
        {
            Console.WriteLine();

            for (var i = 0; i < _eventsQueue.Count; i++)
            {
				Console.ForegroundColor = GetEventsColor(_eventsQueue[i].SourceId);
                Console.WriteLine($"[{i + 1}] {_eventsQueue[i]}");
				Console.ResetColor();
            }

            Console.WriteLine("Press any key to return to the menu...");
            Console.ReadKey();
        }

		private static ConsoleColor GetEventsColor(Guid id)
		{
			if (ColorMap.TryGetValue(id, out var color))
				return color;

			color = PetitionColors[CurrentColor];
			ColorMap.Add(id, color);

			if (CurrentColor == PetitionColors.Length - 1) CurrentColor = 0;
			else CurrentColor++;

			return color;
		}

        private static void ShowMenu()
        {
            Console.Clear();           
            Console.WriteLine("1) Create Petition");
            Console.WriteLine("2) View all petitions");
            Console.WriteLine("3) Sign petition");
            Console.WriteLine("4) Revoke sign");
            Console.WriteLine("5) Close petition");
            Console.WriteLine("6) View events");
            Console.WriteLine();
            Console.WriteLine("9) Exit");
            Console.Write("Choose an option from the menu: ");
        }

        private static Guid GetPetitionGuid()
        {
            Console.Write("Choose petition: ");
            var petitionId = int.Parse(Console.ReadLine());

            var petitions = _queryService.GetAll();

            var petitionGuid = petitions[petitionId - 1].Id;
            return petitionGuid;
        }

        private static void Initialize()
        {
            _commandBus.Subscribe<CreatePetition>(_commandHandler);
            _commandBus.Subscribe<SignPetition>(_commandHandler);
            _commandBus.Subscribe<RevokeSign>(_commandHandler);
            _commandBus.Subscribe<ClosePetition>(_commandHandler);
        }
    }
}

