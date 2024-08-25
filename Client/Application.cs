using Orion.Client.Users.Services;
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
    public class Application
    {

        private Dictionary<int, Tuple<string, Action>> _availableOptions;

        private readonly IUserService _userService;

        public Application(IUserService userService)
        {
            _availableOptions = new Dictionary<int, Tuple<string, Action>>();
            _userService = userService;
        }

        public async Task Run()
        {
            _availableOptions[0] = new Tuple<string, Action>("Login", async () =>
            {
                Console.WriteLine("Enter username: ");
                string username = Console.ReadLine();

                var subscriptable = await _userService.AuthenticateUser(new Credentials() { Username = username });
                subscriptable.Subscribe(result =>
                {
                    if (result.OperationInformation.OperationMessageCode == AuthenticationMessages.VALID_CREDENTIALS)
                        Console.WriteLine(AuthenticationMessages.VALID_CREDENTIALS);
                    else
                        Console.WriteLine("Invalid credentials entered...");
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
