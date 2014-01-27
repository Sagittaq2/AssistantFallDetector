using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Input;
using Microsoft.Phone.Controls;
using Microsoft.Phone.UserData;
using Microsoft.Phone.Shell;
using AssistantFallDetector.Resources;
using AssistantFallDetector.Models;
using AssistantFallDetector.ViewModels;

namespace AssistantFallDetector
{
    public partial class MainPage : PhoneApplicationPage
    {

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string strItemIndex;

            if (NavigationContext.QueryString.TryGetValue("goto", out strItemIndex))
                MyPivot.SelectedIndex = Convert.ToInt32(strItemIndex);

            base.OnNavigatedTo(e);

            //if (ApplicationSettingsData.InitialLaunchSetting)
            //{
            //    // App has initialized for the first time.
            //    ApplicationSettingsData.InitialLaunchSetting = false;
            //}

        }

        private void ApplicationBarIconAboutButton_Click(object sender, EventArgs e)
        {
            var viewModel = (VMMainPage)this.DataContext;
            viewModel.AboutCommand.Execute(null);
        }

        private void MediaElement1_Loaded(object sender, RoutedEventArgs e)
        {

        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}