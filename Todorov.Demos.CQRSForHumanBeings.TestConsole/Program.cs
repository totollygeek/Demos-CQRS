using System;
using System.Collections.Generic;
using Spectre.Console;
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
        private static List<IVersionedEvent> _eventsQueue = new();

		private static ConsoleColor[] PetitionColors { get; } =
		{
			ConsoleColor.Cyan,
			ConsoleColor.Green,
			ConsoleColor.Magenta
		};

		private static int CurrentColor { get; set; } = 0;

		private static Dictionary<Guid, ConsoleColor> ColorMap { get; } = new();

		private static readonly IEventBus _eventBus = new SimpleEventBus();
        private static readonly ICommandBus _commandBus = new SimpleCommandBus();

        private static readonly IEventSourceRepository<PetitionAggregate> _eventSourceRepository =
            new InMemoryEventSourceRepository<PetitionAggregate>(_eventBus, events => _eventsQueue.AddRange(events));
        private static readonly IReadModelStore<PetitionModel> _readModelStore =
            new InMemoryReadModelStore();

        private static readonly PetitionCommandHandler _commandHandler = new(_eventSourceRepository);
        private static PetitionListHandler _petitionListHandler = new PetitionListHandler(_eventBus, _readModelStore);

        private static readonly PetitionService _service = new(_commandBus);
        private static readonly PetitionQueryService _queryService = new(_readModelStore);
        #endregion

        static void Main()
        {
            Initialize();

            while(true)
            {
                var selectedItem = ShowMenu();
                selectedItem.Item2();
                Console.WriteLine("Press any key to return to the menu...");
                Console.ReadKey();
            }
        }

        private static void Exit()
        {
            Console.WriteLine();
            Console.Write("Bye Bye!");
            Environment.Exit(0);
        }
        

        private static void CreatePetition()
        {
            Console.WriteLine();
            Console.Write("Enter title: ");
            var title = Console.ReadLine();

            _service.CreatePetition(title);

            Console.WriteLine("Petition created!");
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
        }

        private static void ClosePetition()
        {
            Console.WriteLine();
            Guid petitionGuid = GetPetitionGuid();

            _service.ClosePetition(petitionGuid);

            Console.WriteLine("Petition closed!");
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

        private static Tuple<string, Action> ShowMenu()
        {
            AnsiConsole.Clear();
            return AnsiConsole.Prompt(
                new SelectionPrompt<Tuple<string, Action>>()
                    .Title("Choose what you want to do:")
                    .PageSize(10)
                    .AddChoices(
                        new Tuple<string, Action>("Create Petition", CreatePetition),
                        new Tuple<string, Action>("View all petitions", ViewPetitions),
                        new Tuple<string, Action>("Sign petition", SignPetition),
                        new Tuple<string, Action>("Revoke sign", RevokeSign),
                        new Tuple<string, Action>("Close petition", ClosePetition),
                        new Tuple<string, Action>("View events", ViewEvents),
                        new Tuple<string, Action>("Exit", Exit))
                    .UseConverter(s => s.Item1));
        }

        private static Guid GetPetitionGuid()
        {
            var petitions = _queryService.GetAll();
            var selectedPetition = AnsiConsole.Prompt(
                new SelectionPrompt<PetitionModel>()
                    .Title("Choose petition:")
                    .PageSize(10)
                    .AddChoices(petitions)
                    .UseConverter(s => s.Title));

            return selectedPetition.Id;
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

