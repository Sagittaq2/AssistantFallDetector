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
using AssistantFallDetector.Entities;

namespace AssistantFallDetector
{
    public partial class MainPage : PhoneApplicationPage
    {
        FilterKind contactFilterKind = FilterKind.None;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            contactFilterName.IsChecked = true;

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (ApplicationSettingsData.InitialLaunchSetting)
            {
                // App has initialized for the first time.
                ApplicationSettingsData.InitialLaunchSetting = false;
            }

            if (e.NavigationMode == NavigationMode.New)
            {
                ApplicationBar appBar = new ApplicationBar
                {
                    Mode = ApplicationBarMode.Minimized,
                };

                ApplicationBarMenuItem refreshMenu = new ApplicationBarMenuItem(AppResources.MainPageAppBarRefreshText);

                //refreshMenu.Click += OnRefreshClick;

                appBar.MenuItems.Add(refreshMenu);

                this.ApplicationBar = appBar;
            }

            // Loading settings
            SettingsPanel.DataContext = App.ApplicationSettings;
        }

        private void FilterChange(object sender, RoutedEventArgs e)
        {
            String option = ((RadioButton)sender).Name;

            InputScope scope = new InputScope();
            InputScopeName scopeName = new InputScopeName();

            switch (option)
            {
                case "contactFilterName":
                    contactFilterKind = FilterKind.DisplayName;
                    scopeName.NameValue = InputScopeNameValue.Text;
                    break;

                case "contactFilterPhone":
                    contactFilterKind = FilterKind.PhoneNumber;
                    scopeName.NameValue = InputScopeNameValue.TelephoneNumber;
                    break;

                case "contactFilterEmail":
                    contactFilterKind = FilterKind.EmailAddress;
                    scopeName.NameValue = InputScopeNameValue.EmailSmtpAddress;
                    break;

                default:
                    contactFilterKind = FilterKind.None;
                    break;
            }

            scope.Names.Add(scopeName);
            contactFilterString.InputScope = scope;
            contactFilterString.Focus();
        }

        private void SearchContacts_Click(object sender, RoutedEventArgs e)
        {
            //Add code to validate the contactFilterString.Text input.

            contactResultsLabel.Text = AppResources.MainPageContactsLoadingLabelText1;
            contactResultsData.DataContext = null;

            Contacts cons = new Contacts();

            cons.SearchCompleted += new EventHandler<ContactsSearchEventArgs>(Contacts_SearchCompleted);

            cons.SearchAsync(contactFilterString.Text, contactFilterKind, "Contacts Test #1");
        }


        void Contacts_SearchCompleted(object sender, ContactsSearchEventArgs e)
        {
            //MessageBox.Show(e.State.ToString());

            try
            {
                //Bind the results to the list box that displays them in the UI
                contactResultsData.DataContext = e.Results;
            }
            catch (System.Exception)
            {
                //That's okay, no results
            }

            if (contactResultsData.Items.Count > 0)
            {
                contactResultsLabel.Text = AppResources.MainPageContactsLoadingLabelText2;
            }
            else
            {
                contactResultsLabel.Text = AppResources.MainPageContactsLoadingLabelText3;
            }
        }


        private void ContactResultsData_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.con = ((sender as ListBox).SelectedValue as Contact);

            if (App.con != null)
                NavigationService.Navigate(new Uri("/Views/ContactDetails.xaml", UriKind.Relative));
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