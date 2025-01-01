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
		private bool _hideGroupForm;

		private DispatcherQueue _dispatcherQueue;

		public GroupsListViewModel()
		{
			Groups = new();
			_hideGroupForm = true;

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

			HideGroupForm = true;
        }
	}
}
