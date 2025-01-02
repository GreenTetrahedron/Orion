using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Client.App.Groups.Models
{
    public partial class GroupProfile : ObservableObject
    {
        [ObservableProperty]
        private Guid _groupId;

        [ObservableProperty]
        private string _groupName;
    }
}
