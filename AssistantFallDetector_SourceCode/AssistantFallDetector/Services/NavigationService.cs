using AssistantFallDetector.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.UserData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace AssistantFallDetector.Services
{
    public class NavigationService : INavigationService
    {
        private Action<NavigationEventArgs> onNavigated;

        // Limpia el historial de navegación entre páginas
        public void ClearNavigationHistory()
        {
            PhoneApplicationFrame rootFrame = Application.Current.RootVisual as PhoneApplicationFrame;
            while (rootFrame.RemoveBackEntry() != null) ;
        }

        public void NavigateToContactDetailsPage(object contact)
        {
            this.onNavigated = (e) =>
            {
                var vm = (VMContactDetailsPage)(e.Content as ContactDetailsPage).DataContext;

                vm.Contact = (Contact)contact;
            };
            App.RootFrame.Navigated += RootFrame_Navigated;
            App.RootFrame.Navigate(new Uri("/Views/ContactDetailsPage.xaml", UriKind.Relative));
        }

        public void NavigateToMainPage(object telefono)
        {
            this.onNavigated = (e) =>
            {
                var vm = (VMMainPage)(e.Content as MainPage).DataContext;

                vm.Telefono = (ContactPhoneNumber)telefono;
            };
            App.RootFrame.Navigated += RootFrame_Navigated;
            App.RootFrame.Navigate(new Uri("/Views/MainPage.xaml?goto=3", UriKind.Relative));
        }

        void RootFrame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            App.RootFrame.Navigated -= RootFrame_Navigated;

            if (this.onNavigated != null)
                this.onNavigated(e);

            this.onNavigated = null;
        }
    }
}
