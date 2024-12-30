using Microsoft.EntityFrameworkCore;
using Orion.Cryptography.HashingServices;
using Orion.Logging.LoggingServices;
using Orion.Models.DirectCommunicationModels;
using Orion.Models.GroupModels;
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
                .Include(user => user.Groups)
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
                    }).ToList(),
                GroupProfiles = getDataQueryResult.Groups
                    .Select(group => new GroupProfile()
                    {
                        GroupId = group.GroupId
                    }).ToList()
            };

            return (user == null)
                ? ServerTransmissionService.NewSuccessfulResponseServerTransmission("AuthenticateUserResult", AuthenticationMessages.INVALID_CREDENTIALS)
                    .AddResponseOperationMessage("Internal Server error")
                : ServerTransmissionService.NewSuccessfulResponseServerTransmission("AuthenticateUserResult", AuthenticationMessages.VALID_CREDENTIALS, user)
                    .AddResponseAffectedUser(user.UserId)
                    .AddResponseOperationMessage("Valid credentials entered");
        }

        public async Task<UserDTO?> AddUser(UserInformation userInformation)
        {
            var passwordHash = _hashingService.Hash(Encoding.UTF8.GetBytes(userInformation.Password));
            var user = new User()
            {
                UserId = Guid.NewGuid(),
                Username = userInformation.Username,
                PasswordHash = passwordHash,
                Role = userInformation.Role,
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

        public async Task<ServerResult> AuthenticateSuperadmin(Credentials credentials)
        {
            var passwordHash = _hashingService.Hash(Encoding.UTF8.GetBytes(credentials.Password));

            var getDataQuery = _database.Users
                .Where(user => user.Role == Roles.SUPERADMIN)
                .Where(user => user.Username == credentials.Username && user.PasswordHash == passwordHash)
                .Include(user => user.DirectCommunications)
                .ThenInclude(d => d.Members);

            var getDataQueryResult = await getDataQuery
                .SingleOrDefaultAsync();

            // I dont like the repetition
            if (getDataQueryResult == null)
                return new ServerResult(new OperationInformation<AuthenticationMessages>(Statuses.SUCCEEDED, AuthenticationMessages.INVALID_CREDENTIALS, "Invalid credentials entered..."));

            var user = new UserDTO()
            {
                Username = getDataQueryResult.Username,
                UserId = getDataQueryResult.UserId
            };

            return new ServerResult(new OperationInformation<AuthenticationMessages>(Statuses.SUCCEEDED, AuthenticationMessages.VALID_CREDENTIALS, "Valid credentials"), user);
        }

        public async Task<int> UpdateUser(UserInformation newUserInformation)
        {
            _database.Users.Update(new User()
            {
                UserId = newUserInformation.UserId,
                Username = newUserInformation.Username,
                PasswordHash = _hashingService.Hash(Encoding.UTF8.GetBytes(newUserInformation.Password)),
                Role = newUserInformation.Role
            });

            return await _database.SaveChangesAsync();
        }

        public async Task<UserInformation?> GetUserInformation(Guid userId)
        {
            var user = await _database.Users
                .Where(user => user.UserId == userId)
                .Select(user => new UserInformation()
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    Role = user.Role
                })
                .SingleOrDefaultAsync();

            return user;
        }

        public async Task<List<UserDTO>> GetAllUsers()
        {
            var users = await _database.Users
                .Select(user => new UserDTO()
                {
                    UserId = user.UserId,
                    Username = user.Username
                })
                .ToListAsync();

            return users;
        }
    }
}