using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Orion.Client.DirectCommunications.Services;
using Orion.Models.ClientTransmissions;
using Orion.Models.ServerTransmissions.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Orion.Client.App.DirectCommunications
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NewDirectCommunciation : Page
    {
        private readonly IDirectCommunicationService _directCommunicationService;

        public NewDirectCommunciation()
        {
            this.InitializeComponent();

            _directCommunicationService = App.Current.ConfigurationService.GetInstanceOfType<IDirectCommunicationService>();
        }

        public async void CreateNewDirectCommunication(object sender, RoutedEventArgs e)
        {
            var receiverName = RecipientNameTextBox.Text;
            var newDirectCommunication = new NewDirectCommunicationDTO() { SenderId = App.Current.User.UserId, ReceiverName = receiverName };

            var subscriptable = await _directCommunicationService.NewDirectCommunication(newDirectCommunication);
            subscriptable.Subscribe(result =>
            {
            });
        }
    }
}
