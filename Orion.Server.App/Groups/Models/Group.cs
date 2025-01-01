using CommunityToolkit.Mvvm.ComponentModel;
using Orion.Server.App.Users.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Server.App.Groups.Models
{
    public partial class Group : ObservableObject
    {
        [ObservableProperty]
        private Guid _groupId;

        [ObservableProperty]
        private string _groupName;

        private ObservableCollection<UserProfile> _memberProfiles;

        public ObservableCollection<UserProfile> MemberProfiles
        {
            get => _memberProfiles;
            set
            {
                SetProperty(ref _memberProfiles, value);
            }
        }

        public Group()
        {
            MemberProfiles = new();
        }
    }
}
