using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using Orion.Client.Messages.Services;
using System.Security.Cryptography;
using Orion.Client.App.DirectCommunications;
using Orion.Client.DirectCommunications.Services;
using Orion.Models.MessageModels;
using Orion.Models.ServerTransmissions.Results.Messages;
using Orion.Client.App.Users;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Orion.Client.App.Messages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MessageListPage : Page
    {
        public ObservableCollection<MessageViewModel> Messages { get; set; }

        public Guid DirectCommunicationId { get; set; }

        private readonly IMessageService _messageService;
        private readonly IDirectCommunicationService _directCommunicationService;

        public MessageListPage()
        {
            InitializeComponent();
            _messageService = App.Current.ConfigurationService.GetInstanceOfType<IMessageService>();
            _directCommunicationService = App.Current.ConfigurationService.GetInstanceOfType<IDirectCommunicationService>();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is not DirectCommunicationProfileViewModel)
                return;

            var direct = e.Parameter as DirectCommunicationProfileViewModel;

            DirectCommunicationId = direct.DirectCommunicationId;

            List<MessageDTO> messages = new List<MessageDTO>();

            var subscriptable = await _directCommunicationService.GetMessagesByDirectCommunicationId(DirectCommunicationId);
            subscriptable.Subscribe(result =>
            {
                if (result.OperationInformation.OperationMessageCode != GetMessageMessages.SUCCESSFULLY_RETRIEVED_MESSAGE)
                {
                    return;
                }

                messages = result.Data as List<MessageDTO>;
            });

            messages.ForEach(message =>
            {
                Messages.Add(new MessageViewModel()
                {
                    MessageId = message.MessageId,
                    Content = message.Content,
                    SenderProfile = new UserProfileViewModel()
                    {
                        UserId = message.SenderProfile.UserId,
                        Username = message.SenderProfile.Username
                    }
                });
            });
        }

        private async void SendMessage(object sender, RoutedEventArgs e)
        {
            string content = messageContentTextBox.Text;

            await _messageService.SendDirectMessage(new NewDirectMessage()
            {
                Content = content,
                DirectCommunicationId = DirectCommunicationId,
                SenderId = App.Current.UserViewModel.UserId
            });
        }
    }
}
