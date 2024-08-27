using Orion.Client.DirectCommunications.Services;
using Orion.Client.Transmissions;
using Orion.Client.Users.Services;
using Orion.Models;
using Orion.Models.ClientTransmissions;
using Orion.Models.ServerTransmissions.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Client
{
    public class TestApplication
    {

        private Dictionary<int, Tuple<string, Action>> _availableOptions;

        private readonly IUserService _userService;
        private readonly IDirectCommunicationService _directCommunicationService;

        private readonly ITransmissionService _transmissionService;

        private User sender;

        public TestApplication(IUserService userService, IDirectCommunicationService directCommunicationService, ITransmissionService transmissionService)
        {
            _availableOptions = new Dictionary<int, Tuple<string, Action>>();
            _userService = userService;
            _directCommunicationService = directCommunicationService;

            _transmissionService = transmissionService;
        }

        public async Task Run()
        {
            _transmissionService.InitialiseRouterConnection();

            Console.WriteLine("Connection initialised...");

            Task.Run(async () =>
            {
                var transmission = await _transmissionService.ReceiveData();
                Console.WriteLine($"New transmission of topic: {transmission.Topic}");
            });

            _availableOptions[0] = new Tuple<string, Action>("Login", async () =>
            {
                Console.WriteLine("Enter username: ");
                string username = Console.ReadLine();

                var subscriptable = await _userService.AuthenticateUser(new Credentials() { Username = username });
                subscriptable.Subscribe(result =>
                {
                    Console.WriteLine(result.OperationInformation.OperationMessage);
                    if (result.OperationInformation.OperationMessageCode != AuthenticationMessages.VALID_CREDENTIALS)
                        return;

                    sender = result.Data as User;

                    _availableOptions.Add(2, new Tuple<string, Action>("NewDirectCommunication", async () =>
                    {
                        Console.WriteLine("Receiver Name: ");
                        string name = Console.ReadLine();

                        var directCommunication = new NewDirectCommunication()
                        {
                            SenderId = sender.UserId,
                            ReceiverName = name
                        };

                        var subscriptable = await _directCommunicationService.NewDirectCommunication(directCommunication);
                        subscriptable.Subscribe(result =>
                        {
                            if (result.OperationInformation.OperationMessageCode != DirectCommunicationMessages.DIRECT_COMMUNICATION_CREATION_SUCCEEDED)
                                return;


                        });
                    }));
                });
            });

            _availableOptions[1] = new Tuple<string, Action>("Quit", () => Environment.Exit(0));

            while (true)
            {
                Console.WriteLine("Pick an option: ");

                for (int i = 0; i < _availableOptions.Count; i++)
                {
                    Console.WriteLine($"{i}. {_availableOptions[i].Item1}");
                }

                bool validOptionSelected = int.TryParse(Console.ReadLine(), out var option);

                if (!validOptionSelected)
                    continue;

                _availableOptions[option].Item2.Invoke();
            }
        }
    }
}
