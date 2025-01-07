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
        private User _user;

        [ObservableProperty]
        private bool _hideAddUserForm;

        [ObservableProperty]
        private bool _hideEditUserForm;

        [ObservableProperty]
        private bool _hideDeleteUserPopup;

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

        public void AddUser(User user)
        {
            _dispatcherQueue.TryEnqueue(async () =>
            {
                var addedUser = await App.Current.ConfigurationService.GetInstanceOfType<IUserRepository>()
                    .AddUser(new UserInformation()
                    {
                        Username = user.Username,
                        Password = user.Password,
                        Role = user.Role
                    });

                Users.Add(new Models.UserProfile()
                {
                    UserId = addedUser.UserId,
                    Username = addedUser.Username
                });
                
                HideAddUserForm = true;
            });

        }

        public void OnEditUser(Guid userId)
        {
            _dispatcherQueue.TryEnqueue(async () =>
            {
                HideEditUserForm = false;

                var user = await App.Current.ConfigurationService.GetInstanceOfType<IUserRepository>()
                    .GetUserInformation(userId);

                User = new User()
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    Password = user.Password,
                    Role = user.Role
                };
            });
        }

        public async void OnDeleteUser(Guid userId)
        {
            await DeleteUser(userId);
        }

        public async Task DeleteUser(Guid userId)
        {
            var result = await App.Current.ConfigurationService.GetInstanceOfType<IUserRepository>()
                .DeleteUserById(userId);

            if (!(result == true))
                return;

            Users.Remove(Users.First(user => user.UserId == userId));
        }

        public void UpdateUser(User user)
        {
            _dispatcherQueue.TryEnqueue(async () =>
            {
                var result = await App.Current.ConfigurationService.GetInstanceOfType<IUserRepository>()
                    .UpdateUser(new UserInformation()
                    {
                        UserId = user.UserId,
                        Username = user.Username,
                        Password = user.Password,
                        Role = user.Role
                    });

                if (result == true)
                    HideEditUserForm = true;
            });

        }

    }
}
