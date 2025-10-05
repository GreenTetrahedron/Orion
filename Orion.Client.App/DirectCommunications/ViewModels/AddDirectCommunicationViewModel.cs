using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using Orion.Client.DirectCommunications.Services;
using Orion.Client.Users.Services;
using Orion.Models.DirectCommunicationModels;
using Orion.Models.ServerTransmissions.Results;
using Orion.Models.ServerTransmissions.Results.Messages;
using Orion.Models.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Client.App.DirectCommunications.ViewModels
{
    public partial class AddDirectCommunicationViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool _isNotOnTimeOut;

        private bool _isValid;
        public bool IsValid
        {
            get => _isValid;
            private set
            {
                SetProperty(ref _isValid, value);
            }
        }

        public readonly int MaxUsernameLength;

        private string _givenUsername;

        public string GivenUsername
        {
            get => _givenUsername;
            set
            {
                SetProperty(ref _givenUsername, value[..Math.Min(MaxUsernameLength, value.Length)]);
            }
        }

        private string _errorMessage;

        public string ErrorMessage
        {
            get => _errorMessage;
            private set
            {
                SetProperty(ref _errorMessage, value);
            }
        }

        public AddDirectCommunicationViewModel()
        {
            IsValid = true;
            IsNotOnTimeOut = true;
            MaxUsernameLength = 100;
            ErrorMessage = "User not found";
            GivenUsername = "";
        }

        public void ClearForm()
        {
            GivenUsername = "";
            ErrorMessage = "";
            IsValid = true;
            IsNotOnTimeOut = true;
        }

        public void AddDirectCommunication(Action onCompletedSuccessfully)
        {
            IsValid = true;
            IsNotOnTimeOut = false;
            Timer.Wait(0.3f, () => IsNotOnTimeOut = true);
            var otherUserName = GivenUsername;

            FindUserByName(otherUserName, async id =>
            {
                var subscriptable = await App.Current.ConfigurationService.GetSingletonOfType<IDirectCommunicationService>().NewDirectCommunication(new NewDirectCommunication()
                {
                    ReceiverId = id,
                    SenderId = App.Current.CurrentUser.UserProfile.UserId
                });

                subscriptable.Subscribe(result =>
                {
                    if (result.OperationInformation.OperationMessageCode != DirectCommunicationMessages.DIRECT_COMMUNICATION_CREATION_SUCCEEDED)
                        return;

                    IsValid = true;
                    ErrorMessage = "";
                    GivenUsername = "";
                    onCompletedSuccessfully();
                });
            });
        }

        private async void FindUserByName(string username, Action<Guid> onCompletedSuccessfully)
        {
            if (username == App.Current.CurrentUser.UserProfile.Username)
            {
                ErrorMessage = "User cannot be yourself!";
                IsValid = false;
                return;
            }

            if (App.Current.CurrentUser.DirectCommunications.Any(directCommunication => directCommunication.ReceiverProfile.Username == username))
            {
                ErrorMessage = "DM with user already exists!";
                IsValid = false;
                return;
            }

            var subscriptable = await App.Current.ConfigurationService.GetSingletonOfType<IUserService>().GetUserByUsername(username);

            subscriptable.Subscribe(result =>
            {
                if (result.OperationInformation.Status == Statuses.FAILED || result.OperationInformation.OperationMessageCode != GetUserMessages.USER_FOUND)
                {
                    ErrorMessage = "Something went wrong";
                    IsValid = false;

                    if (result.OperationInformation.OperationMessageCode == GetUserMessages.USER_NOT_FOUND)
                    {
                        ErrorMessage = "User not found";
                    }

                    return;
                }

                var user = (UserProfile)result.Data;

                onCompletedSuccessfully(user.UserId);
            });
        }
    }
}
