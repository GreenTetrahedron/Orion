using Microsoft.EntityFrameworkCore;
using Orion.Models;
using Orion.Models.ClientTransmissions;
using Orion.Models.DirectCommunicationModels;
using Orion.Models.MessageModels;
using Orion.Models.ServerTransmissions;
using Orion.Models.ServerTransmissions.Results;
using Orion.Models.UserModels;
using Orion.Server.DataLayer;
using Orion.Server.DirectCommunications;
using Orion.Server.Messages;
using Orion.Server.ServerTransmissionServices;

namespace Orion.Server.Users.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly OrionDbContext _database;

        public UserRepository(OrionDbContext database)
        {
            _database = database;
        }

        public async Task<ServerTransmission> AuthenticateUser(Credentials credentials)
        {
            var user = await _database.Users
                .Where(user => user.Username == credentials.Username)
                .Select(user => new UserDTO
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    DirectCommunicationProfiles = user.DirectCommunications
                        .Select(directCommunication => (DirectCommunicationProfile)directCommunication)
                        .ToList()
                })
                .SingleOrDefaultAsync();

            return (user == null)
                ? ServerTransmissionService.NewSuccessfulResponseServerTransmission("AuthenticateUserResult", AuthenticationMessages.INVALID_CREDENTIALS)
                    .AddResponseOperationMessage("Invalid credentials entered")
                : ServerTransmissionService.NewSuccessfulResponseServerTransmission("AuthenticateUserResult", AuthenticationMessages.VALID_CREDENTIALS, user)
                    .AddResponseAffectedUser(user.UserId)
                    .AddResponseOperationMessage("Valid credentials entered");
        }

        public async Task<UserDTO?> AddUser(string username)
        {
            var user = new User()
            {
                UserId = Guid.NewGuid(),
                Username = username
            };

            await _database.Users.AddAsync(user);

            int appliedChanges = await _database.SaveChangesAsync();

            bool wasSuccessful = appliedChanges > 0;

            return wasSuccessful ? (UserDTO)user : null;
        }

        public async Task<UserDTO?> GetUser(Guid userId)
        {
            return await _database.Users
                .Where(user => user.UserId == userId)
                .Select(user => new UserDTO
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    DirectCommunicationProfiles = user.DirectCommunications
                        .Select(directCommunication => (DirectCommunicationProfile)directCommunication)
                        .ToList()
                })
                .SingleOrDefaultAsync();
        }
    }
}