using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Orion.Models.DirectCommunicationModels;
using Orion.Models.GroupModels;
using Orion.Models.MessageModels;
using Orion.Models.ServerTransmissions;
using Orion.Models.ServerTransmissions.Results;
using Orion.Models.ServerTransmissions.Results.Messages;
using Orion.Models.UserModels;
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

        public async Task<GroupDTO?> AddGroup(GroupInformation newGroupInformation)
        {
            List<User> members = new();

            foreach(var memberProfile in newGroupInformation.MemberProfiles)
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
                GroupName = newGroupInformation.Name,
                Members = members
            };

            await _database.Groups.AddAsync(group);

            return await _database.SaveChangesAsync() > 0
                ? new GroupDTO()
                {
                    GroupId = group.GroupId,
                    Name = group.GroupName,
                    MemberProfiles = group.Members.Select(member => new UserProfile { UserId = member.UserId, Username = member.Username }).ToList()
                }
                : null;
        }

        public async Task<bool> DeleteGroupById(Guid id)
        {
            var group = await _database.Groups
                .Include(group => group.Messages)
                .Where(group => group.GroupId == id)
                .SingleOrDefaultAsync();

            if (group == null)
                return false;

            _database.Groups.Remove(group);

            return await _database.SaveChangesAsync() > 0;
        }

        public async Task<List<GroupDTO>> GetAllGroups()
        {
            return await _database.Groups
                .Select(group => new GroupDTO()
                {
                    GroupId = group.GroupId,
                    Name = group.GroupName,
                    MemberProfiles = group.Members
                        .Select(member => new Models.UserModels.UserProfile()
                        {
                            UserId = member.UserId,
                            Username = member.Username
                        }).ToList()
                })
                .ToListAsync();
        }

        public async Task<GroupDTO?> GetGroupById(Guid id)
        {
            return await _database.Groups
                .Where(group => group.GroupId == id)
                .Select(group => new GroupDTO()
                {
                    GroupId = group.GroupId,
                    Name = group.GroupName,
                    MemberProfiles = group.Members
                        .Select(member => new Models.UserModels.UserProfile()
                        {
                            UserId = member.UserId,
                            Username = member.Username
                        }).ToList()
                })
                .SingleOrDefaultAsync();
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

        public async Task<bool> UpdateGroup(GroupInformation newGroupInformation)
        {
            var group = await _database.Groups
                .Include(group => group.Members)
                .FirstOrDefaultAsync(group => group.GroupId == newGroupInformation.GroupId);

            if (group == null)
                return false;

            group.GroupName = newGroupInformation.Name;

            group.Members = new();
            group.Members.Clear();

            foreach (var member in newGroupInformation.MemberProfiles)
            {
                var user = await _database.Users.FindAsync(member.UserId);

                if (user == null)
                    throw new Exception($"User {member.Username} with id {member.UserId} not found...");

                group.Members.Add(user);
            }

            _database.Update(group);

            return await _database.SaveChangesAsync() > 0;
        }
    }
}
