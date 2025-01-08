using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Dispatching;
using Orion.Server.App.Groups.Models;
using Orion.Server.Groups.Repositories;
using Orion.Server.Migrations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Orion.Server.App.Groups.ViewModels
{
    public partial class GroupsListViewModel : ObservableObject
    {
		private ObservableCollection<Group> _groups;

		public ObservableCollection<Group> Groups
		{
			get { return _groups; }
			private set { SetProperty(ref _groups, value); }
		}

		[ObservableProperty]
		private Group _group;

		[ObservableProperty]
		private bool _hideAddGroupForm;

        [ObservableProperty]
        private bool _hideEditGroupForm;

        private DispatcherQueue _dispatcherQueue;

		public GroupsListViewModel()
		{
			Groups = new();
			HideAddGroupForm = true;
            HideEditGroupForm = true;

            _dispatcherQueue = DispatcherQueue.GetForCurrentThread();
			InitialiseGroups();
		}

		private void InitialiseGroups()
		{
			_dispatcherQueue.TryEnqueue(async () =>
			{
				var groups = await App.Current.ConfigurationService.GetInstanceOfType<IGroupRepository>()
					.GetAllGroups();

				groups.ForEach(group =>
				{
					var newGroup = new Group()
					{
						GroupId = group.GroupId,
						GroupName = group.Name
					};

					group.MemberProfiles.Select(user => new Users.Models.UserProfile()
					{
						UserId = user.UserId,
						Username = user.Username
					}).ToList().ForEach(newGroup.MemberProfiles.Add);

					Groups.Add(newGroup);
                });
			});
		}

		public async Task AddGroup(Group group)
		{
            bool groupAdded = await App.Current.ConfigurationService.GetInstanceOfType<IGroupRepository>()
                .AddGroup(new Orion.Models.GroupModels.GroupInformation()
                {
                    MemberProfiles = group.MemberProfiles.Select(user => new Orion.Models.UserModels.UserProfile()
                    {
                        UserId = user.UserId,
                        Username = user.Username
                    }).ToList(),
                    Name = group.GroupName
                });

			if (!groupAdded)
				return;
			
			Groups.Add(group);

			HideAddGroupForm = true;
        }

		public void OnEditGroup(Guid groupId)
		{
			_dispatcherQueue.TryEnqueue(async () =>
			{
				HideEditGroupForm = false;

				var group = await App.Current.ConfigurationService.GetInstanceOfType<IGroupRepository>()
					.GetGroupById(groupId);

				if (group == null) return;

				Group = new Group()
				{
					GroupId = group.GroupId,
					GroupName = group.Name
				};


				group.MemberProfiles.Select(member => new Users.Models.UserProfile()
				{
					UserId = member.UserId,
					Username = member.Username
				}).ToList().ForEach(Group.MemberProfiles.Add);
			});
        }

        public async Task UpdateGroup(Group newGroup)
        {
            bool groupUpdated = await App.Current.ConfigurationService.GetInstanceOfType<IGroupRepository>()
                .UpdateGroup(new Orion.Models.GroupModels.GroupInformation()
                {
					GroupId = newGroup.GroupId,
                    MemberProfiles = newGroup.MemberProfiles.Select(user => new Orion.Models.UserModels.UserProfile()
                    {
                        UserId = user.UserId,
                        Username = user.Username
                    }).ToList(),
                    Name = newGroup.GroupName
                });

            if (!groupUpdated)
                return;

            Groups[Groups.IndexOf(Groups.First(group => group.GroupId == newGroup.GroupId))] = newGroup;

            HideEditGroupForm = true;
        }
    }
}
