using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Orion.Server.App
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public Frame RootFrame { get => rootFrame; }

        public MainWindow()
        {
            this.InitializeComponent();

            CustomiseTitleBar();

            AppWindow.SetIcon("C:\\__Kabushak\\_Programming\\_Applications\\Orion\\Orion\\Orion.Server.App\\Assets\\AdminPortal.ico");
        }

        private void CustomiseTitleBar()
        {
            if (!AppWindowTitleBar.IsCustomizationSupported())
                return;

            AppWindow.Title = "Orion Admin";

            AppWindow.TitleBar.ForegroundColor = Colors.White;
            AppWindow.TitleBar.BackgroundColor = Color.FromArgb(255, 26, 26, 25);

            AppWindow.TitleBar.ButtonBackgroundColor = Color.FromArgb(255, 26, 26, 25);
            AppWindow.TitleBar.ButtonForegroundColor = Colors.White;

            AppWindow.TitleBar.InactiveForegroundColor = Colors.White;
            AppWindow.TitleBar.InactiveBackgroundColor = Color.FromArgb(255, 26, 26, 25);

            AppWindow.TitleBar.ButtonInactiveBackgroundColor = Color.FromArgb(255, 26, 26, 25);
            AppWindow.TitleBar.ButtonInactiveForegroundColor = Colors.White;

            AppWindow.TitleBar.ButtonHoverBackgroundColor = Color.FromArgb(255, 40, 40, 40);
        }
    }
}
