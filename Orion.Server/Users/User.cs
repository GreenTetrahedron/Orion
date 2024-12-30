using Orion.Models.DirectCommunicationModels;
using Orion.Models.UserModels;
using Orion.Server.DirectCommuncations;
using Orion.Server.Groups;
using Orion.Server.Users;

namespace Orion.Server.Users
{
    public class User
    {
        public Guid UserId { get; set; }

        public Roles Role { get; set; }

        public string Username { get; set; }

        public byte[] PasswordHash { get; set; }

        public List<DirectCommunication> DirectCommunications { get; set; }
        public List<Group> Groups { get; set; }

        public static implicit operator UserProfile(User user) =>
            new UserProfile { UserId = user.UserId, Username = user.Username };

        public static explicit operator UserDTO(User user) =>
            new UserDTO
            {
                UserId = user.UserId,
                Username = user.Username,
                DirectCommunicationProfiles = user.DirectCommunications.Select(directCommunication => (DirectCommunicationProfile)directCommunication).ToList()
            };
    }
}
