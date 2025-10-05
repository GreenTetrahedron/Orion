using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Orion.Client.DirectCommunications.Services;
using Orion.Client.Users.Services;
using Orion.Models.ServerTransmissions.Results.Messages;
using Orion.Models.UserModels;
using Orion.Models.DirectCommunicationModels;
using Orion.Models.ServerTransmissions.Results;
using System.Linq;
using Orion.Client.App.DirectCommunications.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Orion.Client.App.DirectCommunications.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddDirectCommunicationPage : Page
    {
        private AddDirectCommunicationViewModel _model;

        public AddDirectCommunicationViewModel Model
        {
            get { return _model; }
            set { _model = value; }
        }


        public event Action OnAddDirectCommunicationSuccessful = delegate { };

        public AddDirectCommunicationPage()
        {
            InitializeComponent();
            Model ??= new();
        }

        private void AddDirectCommunication(object sender, RoutedEventArgs e)
        {
            Model.AddDirectCommunication(OnAddDirectCommunicationSuccessful);
        }
    }
}
