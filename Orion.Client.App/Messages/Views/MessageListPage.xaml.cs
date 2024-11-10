using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Orion.Client.App.DirectCommunications.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Orion.Models.MessageModels;
using Orion.Client.App.Messages.Models;
using System.Collections.ObjectModel;
using Orion.Client.Messages.Services;
using System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Orion.Client.App.Messages.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public partial class MessageListPage : Page, INotifyPropertyChanged
    {
        public DirectCommunication DirectCommunication { get; set; }

        public ObservableCollection<Message> Messages { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private readonly IMessageService _messageService;

        public MessageListPage()
        {
            InitializeComponent();
            _messageService = App.Current.ConfigurationService.GetSingletonOfType<IMessageService>();
        }

        private void SendMessage(object sender, RoutedEventArgs e)
        {
            var content = messageEntryTextEntry.Text;

            var subscriptable = _messageService.SendDirectMessage(new NewDirectMessage()
            {
                Content = content,
                DirectCommunicationId = DirectCommunication.DirectCommunicationId,
                SenderId = App.Current.CurrentUser.UserProfile.UserId,
                LastUpdated = DateTime.Now
            });
        }
    }
}
