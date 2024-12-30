using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Orion.Models.DirectCommunicationModels;
using Orion.Models.GroupModels;
using Orion.Models.MessageModels;
using Orion.Models.ServerTransmissions;
using Orion.Models.ServerTransmissions.Results;
using Orion.Models.ServerTransmissions.Results.Messages;
using Orion.Server.DataLayer;
using Orion.Server.Exceptions;
using Orion.Server.ServerTransmissionServices;
using Orion.Server.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Server.Groups.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private readonly OrionDbContext _database;

        public GroupRepository(OrionDbContext database)
        {
            _database = database;
        }

        public async Task AddGroup(GroupInformation groupInformation)
        {
            List<User> members = new();

            foreach(var memberProfile in groupInformation.MemberProfiles)
            {
                try
                {
                    members.Add(await _database.Users.FindAsync(memberProfile.UserId));
                }
                catch
                {
                    throw new UserNotFoundException($"{memberProfile.Username} was not found...");
                }
            }

            Group group = new Group()
            {
                GroupId = Guid.NewGuid(),
                Members = members
            };

            await _database.Groups.AddAsync(group);
        }

        public Task<GroupDTO?> GetGroupById(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<ServerTransmission?> GetGroupMessagesByGroupId(GroupId id)
        {
            var messages = await _database.Groups
                .Where(group => group.GroupId == id.Id)
                .Select(group =>
                    group.Messages
                        .OrderBy(message => message.LastUpdated)
                        .Select(message => new MessageDTO
                        {
                            MessageId = message.MessageId,
                            SenderProfile = message.Sender,
                            Content = message.Content,
                            LastUpdated = message.LastUpdated
                        })
                        .ToList()
                ).SingleOrDefaultAsync();

            return messages == null
                ? ServerTransmissionService
                    .NewSuccessfulResponseServerTransmission("GetGroupMessagesByGroupIdResult", GetMessageMessages.NO_MESSAGES_FOUND)
                    .AddResponseOperationMessage("No messages were found in the group")
                : ServerTransmissionService
                    .NewSuccessfulResponseServerTransmission("GetGroupMessagesByGroupIdResult", GetMessageMessages.SUCCESSFULLY_RETRIEVED_MESSAGE, messages)
                    .AddResponseOperationMessage("Messages were successfully retrieved");
        }
    }
}
