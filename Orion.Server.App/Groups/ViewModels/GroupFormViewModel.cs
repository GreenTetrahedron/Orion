using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.UI.Dispatching;
using Orion.Models.ServerTransmissions.Results;
using Orion.Models.ServerTransmissions.Results.Messages;
using Orion.Models.UserModels;
using Orion.Server.App.Groups.Models;
using Orion.Server.App.Users.Models;
using Orion.Server.Groups.Repositories;
using Orion.Server.Users.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using Windows.ApplicationModel.VoiceCommands;

namespace Orion.Server.App.Groups.ViewModels
{
    public partial class GroupFormViewModel : ObservableObject
    {
        [ObservableProperty]
        private Group _group;

        [ObservableProperty]
        private bool _userExists;

        private DispatcherQueue _dispatcherQueue;

        public GroupFormViewModel()
		{
            UserExists = true;
            Group = new();

            _dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        }

		public void TryAddMemberByUsername(string username)
        {
            _dispatcherQueue.TryEnqueue(async () =>
            {
                var result = (ServerResult<GetUserMessages>)(await App.Current.ConfigurationService.GetInstanceOfType<IUserRepository>()
                    .GetUserProfileByUsername(username))
                    .Response.ServerResult;

                if (result.OperationInformation.OperationMessageCode == GetUserMessages.USER_NOT_FOUND)
                {
                    UserExists = false;
                    return;
                }

                UserExists = true;

                var userProfile = result.Data as Orion.Models.UserModels.UserProfile;

                Group.MemberProfiles.Add(new Users.Models.UserProfile()
                {
                    UserId = userProfile.UserId,
                    Username = userProfile.Username
                });
            });
        }

        public void RemoveUserById(Guid userId)
        {
            _dispatcherQueue.TryEnqueue(() =>
            {
                Group.MemberProfiles.Remove(Group.MemberProfiles
                    .Where(user => user.UserId == userId)
                    .SingleOrDefault());
            });
        }

        public void LoadGroupById(Guid groupId)
        {
            _dispatcherQueue.TryEnqueue(async () =>
            {
                var group = await App.Current.ConfigurationService.GetInstanceOfType<IGroupRepository>()
                    .GetGroupById(groupId);

                Group.GroupName = group.Name;
                
                group.MemberProfiles.Select(member =>
                    new Users.Models.UserProfile()
                    {
                        UserId = member.UserId,
                        Username = member.Username
                    }).ToList()
                    .ForEach(Group.MemberProfiles.Add);
            });
        }
	}
}
