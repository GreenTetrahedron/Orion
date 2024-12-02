using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Dispatching;
using Orion.Models.UserModels;
using Orion.Server.App.Users.Models;
using Orion.Server.Users.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Server.App.Users.ViewModels
{
    public partial class UsersListViewModel : ObservableObject
    {
        private ObservableCollection<Models.UserProfile> _users;
        public ObservableCollection<Models.UserProfile> Users
        {
            get => _users;
            private set { SetProperty(ref _users, value); }
        }

        [ObservableProperty]
        private bool _hideAddUserForm;

        [ObservableProperty]
        private bool _hideEditUserForm;

        private DispatcherQueue _dispatcherQueue;

        public UsersListViewModel()
        {
            HideAddUserForm = true;
            HideEditUserForm = true;

            Users = new ObservableCollection<Models.UserProfile>();

            _dispatcherQueue = DispatcherQueue.GetForCurrentThread();

            PopulateUsers();
        }

        private void PopulateUsers()
        {
            _dispatcherQueue.TryEnqueue(async () =>
            {
                var users = await App.Current.ConfigurationService.GetInstanceOfType<IUserRepository>()
                    .GetAllUsers();

                users.ForEach(user =>
                    Users.Add(new Models.UserProfile()
                    {
                        UserId = user.UserId,
                        Username = user.Username
                    })
                );
            });
        }

        public void AddUser(string username, string password, Roles role)
        {
            _dispatcherQueue.TryEnqueue(async () =>
            {
                var user = await App.Current.ConfigurationService.GetInstanceOfType<IUserRepository>()
                    .AddUser(new UserInformation()
                    {
                        Username = username,
                        Password = password,
                        Role = role
                    });

                Users.Add(new Models.UserProfile()
                {
                    UserId = user.UserId,
                    Username = user.Username
                });
                
                HideAddUserForm = true;
            });

        }
    }
}
