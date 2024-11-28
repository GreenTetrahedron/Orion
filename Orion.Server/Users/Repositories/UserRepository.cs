using Microsoft.EntityFrameworkCore;
using Orion.Cryptography.HashingServices;
using Orion.Models.DirectCommunicationModels;
using Orion.Models.ServerTransmissions;
using Orion.Models.ServerTransmissions.Results;
using Orion.Models.ServerTransmissions.Results.Messages;
using Orion.Models.UserModels;
using Orion.Server.DataLayer;
using Orion.Server.ServerTransmissionServices;
using System.Text;

namespace Orion.Server.Users.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly OrionDbContext _database;
        private readonly IHashingService _hashingService;

        public UserRepository(OrionDbContext database, IHashingService hashingService)
        {
            _database = database;
            _hashingService = hashingService;
        }

        public async Task<ServerTransmission> AuthenticateUser(Credentials credentials)
        {
            var passwordHash = _hashingService.Hash(Encoding.UTF8.GetBytes(credentials.Password));

            var getDataQuery = _database.Users
                .Where(user => user.Username == credentials.Username && user.PasswordHash == passwordHash)
                .Include(user => user.DirectCommunications)
                .ThenInclude(d => d.Members);

            var getDataQueryResult = await getDataQuery
                .SingleOrDefaultAsync();

            // I dont like the repetition
            if (getDataQueryResult == null)
                return ServerTransmissionService.NewSuccessfulResponseServerTransmission("AuthenticateUserResult", AuthenticationMessages.INVALID_CREDENTIALS)
                    .AddResponseOperationMessage("Invalid credentials entered");

            var user = new UserDTO()
            {
                Username = getDataQueryResult.Username,
                UserId = getDataQueryResult.UserId,
                DirectCommunicationProfiles = getDataQueryResult.DirectCommunications
                    .Select(directCommunication => new DirectCommunicationProfile()
                    {
                        DirectCommunicationId = directCommunication.DirectCommunicationId,
                        MemberProfiles = directCommunication.Members
                            .Select(member => new UserProfile()
                            {
                                UserId = member.UserId,
                                Username = member.Username
                            }).ToList()
                    }).ToList()
            };

            return (user == null)
                ? ServerTransmissionService.NewSuccessfulResponseServerTransmission("AuthenticateUserResult", AuthenticationMessages.INVALID_CREDENTIALS)
                    .AddResponseOperationMessage("Internal Server error")
                : ServerTransmissionService.NewSuccessfulResponseServerTransmission("AuthenticateUserResult", AuthenticationMessages.VALID_CREDENTIALS, user)
                    .AddResponseAffectedUser(user.UserId)
                    .AddResponseOperationMessage("Valid credentials entered");
        }

        public async Task<UserDTO?> AddUser(Credentials credentials)
        {
            var passwordHash = _hashingService.Hash(Encoding.UTF8.GetBytes(credentials.Password));
            var user = new User()
            {
                UserId = Guid.NewGuid(),
                Username = credentials.Username,
                PasswordHash = passwordHash,
                DirectCommunications = new List<DirectCommuncations.DirectCommunication>()
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
                        .Select(directCommunication => new DirectCommunicationProfile()
                        {
                            DirectCommunicationId = directCommunication.DirectCommunicationId,
                            MemberProfiles = directCommunication.Members
                                .Select(member => (UserProfile)member).ToList()
                        })
                        .ToList()
                })
                .SingleOrDefaultAsync();
        }

        public async Task<ServerTransmission> GetUserProfileByUsername(string username)
        {
            var user = await _database.Users
                .Where(user => user.Username == username)
                .Select(user => new UserProfile()
                {
                    UserId = user.UserId,
                    Username = user.Username
                })
                .SingleOrDefaultAsync();

            return user == null
                ? ServerTransmissionService
                    .NewSuccessfulResponseServerTransmission("GetUserByUsernameResult", GetUserMessages.USER_NOT_FOUND)
                    .AddResponseOperationMessage($"No user found with username: {username}")
                : ServerTransmissionService
                    .NewSuccessfulResponseServerTransmission("GetUserByUsernameResult", GetUserMessages.USER_FOUND, user)
                    .AddResponseOperationMessage($"User with username: {username} was found");
        }

        public async Task<Roles?> GetRoleByUserId(Guid userId)
        {
            var role = await _database.Users
                .Where(user => user.UserId == userId)
                .Select(user => user.Role)
                .SingleOrDefaultAsync();

            return role;

        }
    }
}